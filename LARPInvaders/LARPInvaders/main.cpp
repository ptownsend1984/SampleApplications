#include "Master.h"

SDL_Surface* screen = NULL;
SDL_Surface* tileMap = NULL;
SDL_Color tileMapColor;
SDL_Color backgroundColor;
SDL_Color uiTextColor;

//http://www.fontsquirrel.com/fonts/PT-Sans
TTF_Font* titleFont = NULL;
TTF_Font* textFont = NULL;

//http://www.freesound.org/people/parnellij/sounds/74892/
Mix_Chunk* lightningSound = NULL;
//http://www.freesound.org/people/RandomationPictures/sounds/147017/
Mix_Chunk* splatSound = NULL;
//http://soundbible.com/1952-Punch-Or-Whack.html
Mix_Chunk* whackSound = NULL;

//http://www.soundjay.com/free-music-2.html
Mix_Music* allMusic[MUSIC_TYPES];

Map currentMap;
Camera camera;
GameLevels gameLevels;

Player player;
Enemy enemy[MAX_ENEMIES];

Bullet playerBullets[MAX_PLAYER_BULLET];
Bullet enemyBullets[MAX_ENEMY_BULLET];

int currentLevel;
int levelStartTicks;
int levelEndTicks;

constants::PlayingMode playingMode;
constants::MusicType musicType;

bool init()
{
    //Initialize all SDL subsystems
    if( SDL_Init( SDL_INIT_EVERYTHING ) == -1 )
        return false;

	if(TTF_Init() == -1)
		return false;

    //Set the window caption
    SDL_WM_SetCaption( "LARP Invaders", NULL );

    //Set up the screen
    screen = SDL_SetVideoMode( SCREEN_WIDTH, SCREEN_HEIGHT, SCREEN_BPP, SDL_SWSURFACE );

    //If there was an error in setting up the screen
    if( screen == NULL )
    {
        return false;
    }

	if((Mix_Init(MIX_INIT_OGG) & MIX_INIT_OGG) != MIX_INIT_OGG)
		return false;

	if( Mix_OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 4096) == -1)
	{
		return false;
	}

	backgroundColor.r = 50;
	backgroundColor.g = 50;
	backgroundColor.b = 50;

	tileMapColor.r = 0;
	tileMapColor.g = 0;
	tileMapColor.b = 0;	

	uiTextColor.r = 255;
	uiTextColor.g = 255;
	uiTextColor.b = 255;

	//http://pousse.rapiere.free.fr/tome/
	tileMap = LoadImage("tile.png", tileMapColor);
	if(tileMap == NULL)
		return false;

	titleFont = TTF_OpenFont("PTC55F.ttf", 16);
	if(titleFont == NULL)
		return false;

	TTF_SetFontStyle(titleFont, TTF_STYLE_BOLD);

	textFont = TTF_OpenFont("PTC55F.ttf", 12);
	if(textFont == NULL)
		return false;

	lightningSound = Mix_LoadWAV("lightning.wav");
	if(lightningSound == NULL)
		return false;

	splatSound = Mix_LoadWAV("splat.wav");
	if(splatSound == NULL)
		return false;

	whackSound = Mix_LoadWAV("whack.wav");
	if(whackSound == NULL)
		return false;
	
	allMusic[0] = Mix_LoadMUS("barn-beat.ogg");
	if(allMusic[0] == NULL)
		return false;
	
	allMusic[1] = Mix_LoadMUS("legendaryvirtue.ogg");
	if(allMusic[1] == NULL)
		return false;
	
	allMusic[2] = Mix_LoadMUS("finalbattle.ogg");
	if(allMusic[2] == NULL)
		return false;
	
	allMusic[3] = Mix_LoadMUS("gameover.ogg");
	if(allMusic[3] == NULL)
		return false;
	
	allMusic[4] = Mix_LoadMUS("gamecompleted.ogg");
	if(allMusic[4] == NULL)
		return false;

	currentMap.Load("CastleMap.txt");

	gameLevels.Load("Levels.txt");

	init_genrand((unsigned long)std::time(0));

    //If everything initialized fine
    return true;
}

void shutdown()
{
	//Stop the music
	if(Mix_PlayingMusic() == 1)
	{
		Mix_HaltMusic();
	}

	//Free resources
	for(int i = 0; i < MUSIC_TYPES; i++)
	{
		if(allMusic[1] != NULL)
		{
			Mix_FreeMusic(allMusic[i]);
			allMusic[i] = NULL;
		}
	}
	if(whackSound)
	{
		Mix_FreeChunk(whackSound);
		whackSound = NULL;
	}
	if(splatSound)
	{
		Mix_FreeChunk(splatSound);
		splatSound = NULL;
	}
	if(lightningSound)
	{
		Mix_FreeChunk(lightningSound);
		lightningSound = NULL;
	}
	if(titleFont)
	{
		TTF_CloseFont(titleFont);
		titleFont = NULL;
	}
	if(textFont)
	{
		TTF_CloseFont(textFont);
		textFont = NULL;
	}
	if(tileMap)
	{
		SDL_FreeSurface(tileMap);
		tileMap = NULL;
	}

	Mix_CloseAudio();
	Mix_Quit();
	TTF_Quit();
	SDL_Quit();
}

void ResetGame()
{
	playingMode = constants::Menu;
	levelStartTicks = 0;
	levelEndTicks = 0;
	currentLevel = 0;
	camera.ResetView();

	player.isAlive = true;
	player.hitPoints = 10;	
	player.wWorld = sprite::TILEMAPSIZE;
	player.hWorld = sprite::TILEMAPSIZE;
	player.xWorld = (camera.getRight() - sprite::TILEMAPSIZE) / 2.0;
	player.yWorld = camera.getBottom() - player.hWorld - sprite::TILEMAPSIZE;
	player.lastHitTicks = 0;
	for(int i = 0; i < 4; i++)
		player.keyDownTicks[i] = 0;

	Enemy* currentEnemy;
	for(int i = 0; i < MAX_ENEMIES; i++)
	{
		currentEnemy = &enemy[i];
		currentEnemy->isAlive = false;
		currentEnemy->hitPoints = 0;
		currentEnemy->wWorld = sprite::TILEMAPSIZE;
		currentEnemy->hWorld = sprite::TILEMAPSIZE;
		currentEnemy->xWorld = 0.0;
		currentEnemy->yWorld = 0.0;

		currentEnemy->spawnTicks = 0;
		currentEnemy->lastHitTicks = 0;
		currentEnemy->lastShotTicks = 0;
		currentEnemy->lastActionTicks = 0;
	}

	Bullet* currentBullet;
	for(int i = 0; i < MAX_PLAYER_BULLET; i++)
	{
		currentBullet = &playerBullets[i];
		currentBullet->isAlive = false;
		currentBullet->hitPoints = 0;
		currentBullet->lastMoveTicks = 0;
		currentBullet->spawnTicks = 0;
		currentBullet->wWorld = sprite::TILEMAPSIZE;
		currentBullet->hWorld = sprite::TILEMAPSIZE;
		currentBullet->xVel = 0.0;
		currentBullet->yVel = 0.0;
		currentBullet->xAccel = 0.0;
		currentBullet->yAccel = 0.0;
		currentBullet->ownerID = 0;
	}
	for(int i = 0; i < MAX_ENEMY_BULLET; i++)
	{
		currentBullet = &enemyBullets[i];
		currentBullet->isAlive = false;
		currentBullet->hitPoints = 0;
		currentBullet->lastMoveTicks = 0;
		currentBullet->spawnTicks = 0;
		currentBullet->wWorld = sprite::TILEMAPSIZE;
		currentBullet->hWorld = sprite::TILEMAPSIZE;
		currentBullet->xVel = 0.0;
		currentBullet->yVel = 0.0;
		currentBullet->xAccel = 0.0;
		currentBullet->yAccel = 0.0;
		currentBullet->ownerID = 0;
	}
}

double GetFieldLeft()
{
	return sprite::TILEMAPSIZE;
}
double GetFieldTop()
{
	return sprite::TILEMAPSIZE;
}
double GetFieldRight()
{
	return camera.getRight() - sprite::TILEMAPSIZE;
}
double GetFieldBottom()
{
	return camera.getBottom() - sprite::TILEMAPSIZE;
}

void MoveObject(WorldObject& worldObject, const double xDistance, const double yDistance)
{	
	//Move the xDistance
	if(xDistance != 0.0)
	{
		worldObject.xWorld += xDistance;

		//Block on the left and right wall
		if(worldObject.xWorld < GetFieldLeft())
			worldObject.xWorld = GetFieldLeft();
		else if(worldObject.getRight() > GetFieldRight())
			worldObject.xWorld = GetFieldRight() - worldObject.wWorld;
	}
	if(yDistance != 0.0)
	{
		worldObject.yWorld += yDistance;

		//Block on the top and bottom walls
		if(worldObject.yWorld < GetFieldTop())
			worldObject.yWorld = GetFieldTop();
		else if(worldObject.getBottom() > GetFieldBottom())
			worldObject.yWorld = GetFieldBottom() - worldObject.hWorld;
	}	
}

void SetupNewBullet(Bullet* newBullet, 
	int ownerID,
	double xWorld, double yWorld, double xAccel, double yAccel, 	
	const int frameTicks, sprite::SpriteType spriteType)
{
	newBullet->isAlive = true;		
	newBullet->ownerID = ownerID;
	newBullet->xVel = 0.0;
	newBullet->yVel = 0.0;
	newBullet->xAccel = xAccel;
	newBullet->yAccel = yAccel;
	newBullet->xWorld = xWorld;
	newBullet->yWorld = yWorld;

	newBullet->spawnTicks = frameTicks;
	newBullet->lastMoveTicks = frameTicks;
	newBullet->spriteType = spriteType;
}
bool HasBulletHitWall(Bullet* bullet)
{
	return (bullet->xVel < 0.0 && bullet->xWorld == GetFieldLeft()) || 
		(bullet->xVel > 0.0 && bullet->getRight() == GetFieldRight()) ||
		(bullet->yVel < 0.0 && bullet->yWorld == GetFieldTop()) ||
		(bullet->yVel > 0.0 && bullet->getBottom() == GetFieldBottom());
}

Bullet* GetNextAvailablePlayerBullet()
{
	Bullet* newBullet = NULL;
	for(int i = 0; i < MAX_PLAYER_BULLET; i++)
	{
		if(!playerBullets[i].isAlive)
			newBullet = &playerBullets[i];
	}
	return newBullet;
}
Bullet* GetNextAvailableEnemyBullet(int ownerID, int maxBullets)
{
	Bullet* newBullet = NULL;
	int ownedCount = 0;	
	for(int i = 0; i < MAX_ENEMY_BULLET && ownedCount < maxBullets; i++)
	{
		//If the bullet isn't alive, it is a candidate.
		//Otherwise, tally it if this owner matches.
		if(!enemyBullets[i].isAlive)
		{
			newBullet = &enemyBullets[i];
		}
		else if (enemyBullets[i].ownerID == ownerID)
		{
			ownedCount++;
		}
	}

	//If the owned count is exceeded, return nothing.
	if(ownedCount >= maxBullets)
		newBullet = NULL;

	return newBullet;
}

int HandlePlayerInput(const SDL_Event& inputEvent, const int frameTicks)
{
	Bullet* newBullet = NULL;
	double newBulletX = 0.0;
	double newBulletY = 0.0;
	sprite::SpriteType newBulletSprite;

	if(inputEvent.type == SDL_KEYDOWN)
	{
		switch(inputEvent.key.keysym.sym)
		{
		case SDLK_UP:
			//Shoot bullet up
			newBullet = GetNextAvailablePlayerBullet();
			if(newBullet)
			{				
				newBulletY = -PLAYER_BULLET_ACCEL;
				newBulletSprite = sprite::RedBoltVert;
			}
			break;
		case SDLK_DOWN:
			//Shoot bullet down
			newBullet = GetNextAvailablePlayerBullet();
			if(newBullet)
			{
				newBulletY = PLAYER_BULLET_ACCEL;
				newBulletSprite = sprite::RedBoltVert;
			}
			break;
		case SDLK_LEFT:			
			//Shoot bullet left
			newBullet = GetNextAvailablePlayerBullet();
			if(newBullet)
			{
				newBulletX = -PLAYER_BULLET_ACCEL;
				newBulletSprite = sprite::RedBoltHorizontal;
			}
			break;
		case SDLK_RIGHT:		
			//Shoot bullet right
			newBullet = GetNextAvailablePlayerBullet();
			if(newBullet)
			{
				newBulletX = PLAYER_BULLET_ACCEL;
				newBulletSprite = sprite::RedBoltHorizontal;
			}
			break;
		case SDLK_w:
			//Accelerate up
			player.yAccel = -PLAYER_ACCEL;
			player.keyDownTicks[1] = frameTicks;
			break;
		case SDLK_a:
			//Accelerate left if not moving right
			if(player.xVel == 0.0)
			{
				player.xAccel = -PLAYER_ACCEL;
				player.keyDownTicks[0] = frameTicks;
			}
			break;
		case SDLK_s:
			//Accelerate down if not moving down
			if(player.yVel == 0)
			{
				player.yAccel = PLAYER_ACCEL;
				player.keyDownTicks[1] = frameTicks;
			}
			break;
		case SDLK_d:
			//Accelerate right
			player.xAccel = PLAYER_ACCEL;
			player.keyDownTicks[0] = frameTicks;
			break;
		case SDLK_SPACE:
			break;
		case SDLK_ESCAPE:
			return -1;			
		}
	}
	else if(inputEvent.type == SDL_KEYUP)
	{
		switch(inputEvent.key.keysym.sym)
		{
		case SDLK_w:
			//Halt up movement
			if(player.yVel < 0)
			{
				player.yVel = 0;
				player.yAccel = 0;
				player.keyDownTicks[1] = 0;
			}
			break;
		case SDLK_a:
			//Halt left movement
			if(player.xVel < 0)
			{
				player.xVel = 0;
				player.xAccel = 0;
				player.keyDownTicks[0] = 0;
			}
			break;
		case SDLK_s:
			//Halt down movement
			if(player.yVel > 0)
			{
				player.yVel = 0;
				player.yAccel = 0;
				player.keyDownTicks[1] = 0;
			}
			break;
		case SDLK_d:
			//Halt right movement
			if(player.xVel > 0)
			{
				player.xVel = 0;
				player.xAccel = 0;
				player.keyDownTicks[0] = 0;
			}			
			break;
		}
	}

	//Set up the new bullet
	if(newBullet)
	{
		SetupNewBullet(newBullet, 0, player.xWorld, player.yWorld, newBulletX, newBulletY, frameTicks, newBulletSprite);
		Mix_PlayChannel(-1, lightningSound, 0);
	}
	return 0;
}
int HandleGameInput(const SDL_Event& inputEvent, const int frameTicks)
{
	Bullet* newBullet = NULL;
	if(inputEvent.type == SDL_KEYDOWN)
	{
		switch(inputEvent.key.keysym.sym)
		{
		case SDLK_ESCAPE:
			//Exit
			return -1;		
		case SDLK_RETURN:
			//Start the game in menu mode
			if(playingMode == constants::Menu)
			{
				playingMode = constants::Waiting;
			}
			break;
		}
	}
	return 0;
}
int HandleInput(const SDL_Event& inputEvent, const int frameTicks)
{
	int Result;
	
	Result = HandleGameInput(inputEvent, frameTicks);
	if(Result != 0)
		return Result;

	if(playingMode == constants::Playing)
	{
		Result = HandlePlayerInput(inputEvent, frameTicks);
	}

	return Result;
}
void MoveBullets(Bullet bulletsArray[], int maxBullets, double bulletSpeed, const int frameTicks)
{
	Bullet* currentBullet;
	for(int i = 0; i < maxBullets; i++)
	{
		currentBullet = &bulletsArray[i];
		if(currentBullet->isAlive)
		{
			if(HasBulletHitWall(currentBullet))
			{
				//Bullet hit a wall, remove it
				currentBullet->isAlive = false;
			}
			else
			{
				//Move the bullet if it's alive.
				//First check to see if it has an acceleration
				//and apply it to the velocity.
				if(currentBullet->xAccel != 0.0)
				{
					currentBullet->xVel = currentBullet->xAccel;
					currentBullet->xAccel = 0.0;
				}
				if(currentBullet->yAccel != 0.0)
				{
					currentBullet->yVel = currentBullet->yAccel;
					currentBullet->yAccel = 0.0;
				}

				//Move the bullet according to its velocity
				double xVel, yVel;
				xVel = yVel = 0.0;

				//Don't let the bullet travel too fast
				if(currentBullet->lastMoveTicks > 0 && currentBullet->lastMoveTicks == frameTicks)
				{
					xVel = currentBullet->xVel;
					yVel = currentBullet->yVel;
				}
				else if(currentBullet->lastMoveTicks > 0 && frameTicks - currentBullet->lastMoveTicks > bulletSpeed)
				{
					xVel = currentBullet->xVel;
					yVel = currentBullet->yVel;
					currentBullet->lastMoveTicks = frameTicks;
				}

				if(xVel != 0.0 || yVel != 0.0)
				{
					MoveObject(*currentBullet, xVel, yVel);
				}
			}
		}
	}
}
void MovePlayerBullets(const int frameTicks)
{
	MoveBullets(playerBullets, MAX_PLAYER_BULLET, PLAYER_BULLET_TIME, frameTicks);
}
void MovePlayer(const int frameTicks)
{
	if(player.isAlive)
	{
		//Check to see if the player was struck by an enemy bullet
		Bullet* currentBullet;
		for(int i = 0; i < MAX_ENEMY_BULLET; i++)
		{
			currentBullet = &enemyBullets[i];
			if(currentBullet->isAlive)
			{
				if(currentBullet->isOverlapping(player))
				{
					currentBullet->isAlive = false;

					//The player only can take damage every 300ms
					if(frameTicks - player.lastHitTicks > 300)
					{
						if(player.hitPoints > 1)
						{
							player.lastHitTicks = frameTicks;
							player.hitPoints -= 1;
							Mix_PlayChannel(-1, whackSound, 0);
						}
						else
						{
							player.hitPoints -= 1;
							player.isAlive = false;						
							Mix_PlayChannel(-1, splatSound, 0);
						}
					}
				}
			}
		}

		if(player.isAlive)
		{
			//Move the player.	
			//First check to see if it has an acceleration.
			//If so, apply it to the velocity
			if(player.xAccel != 0.0)
			{
				player.xVel = player.xAccel;
				player.xAccel = 0.0;
			}
			if(player.yAccel != 0.0)
			{
				player.yVel = player.yAccel;
				player.yAccel = 0.0;
			}

			//Move the player according to its velocity
			double xVel, yVel;
			xVel = yVel = 0.0;

			//If moving diagonal, sync the move ticks to prevent the player from zooming
			if(player.keyDownTicks[0] > 0 && player.keyDownTicks[1] > 0 && player.keyDownTicks[0] != player.keyDownTicks[1])
			{
				int min = std::min(player.keyDownTicks[0], player.keyDownTicks[1]);
				player.keyDownTicks[0] = min;
				player.keyDownTicks[1] = min;
			}

			//We don't want to move too fast, so we check the current time against
			//the time the key was first pressed.
			if(player.keyDownTicks[0] > 0 && (frameTicks == player.keyDownTicks[0]))
			{
				xVel = player.xVel;
			}
			else if(player.keyDownTicks[0] > 0 && (frameTicks - player.keyDownTicks[0]) > PLAYER_ACCEL_TIME)
			{
				xVel = player.xVel;
				player.keyDownTicks[0] = frameTicks;
			}

			if(player.keyDownTicks[1] > 0 && (frameTicks == player.keyDownTicks[1]))
			{
				yVel = player.yVel;
			}
			else if(player.keyDownTicks[1] > 0 && (frameTicks - player.keyDownTicks[1]) > PLAYER_ACCEL_TIME)
			{
				yVel = player.yVel;
				player.keyDownTicks[1] = frameTicks;
			}

			if(xVel != 0.0 || yVel != 0.0)
			{
				MoveObject(player, xVel, yVel);
			}
		}
	}
}
void PlayerActions(const int frameTicks)
{
	MovePlayer(frameTicks);
	MovePlayerBullets(frameTicks);
}

void MoveEnemyBullets(const int frameTicks)
{
	MoveBullets(enemyBullets, MAX_ENEMY_BULLET, ENEMY_BULLET_TIME, frameTicks);
}
void MoveEnemyAITriggerHappy(Enemy* currentEnemy, int currentIndex, const int frameTicks, double& xMove, double& yMove)
{
	//This AI is MUCH more likely to shoot than move or do nothing
	if(frameTicks - currentEnemy->lastActionTicks > ENEMY_ACTION_TIME)
	{
		double angle = currentEnemy->getAngleTo(player);

		double Random = genrand_real3();
		if(Random < .12)
		{
			//Consider the angle between the enemy and the player.
			//If the player is more left than up, we want to move left.
			if(!currentEnemy->isAdjacent(player))
			{
				if(0.0 <= angle && angle < 45.0)
				{
					//Player is to the right
					xMove = ENEMY_ACCEL;
				}
				else if(45.0 <= angle && angle < 135.0)
				{									
					if(currentEnemy->getYCenter() > player.getYCenter())
					{
						//Player is above
						yMove = -ENEMY_ACCEL;
					}
					else
					{
						//Player is below
						yMove = ENEMY_ACCEL;
					}
				}
				else
				{
					//Player is to the left
					xMove = -ENEMY_ACCEL;
				}
			}
		}
		else if(Random < .85)
		{
			Bullet* newBullet = NULL;
			double newBulletX = 0.0;
			double newBulletY = 0.0;
			sprite::SpriteType newBulletSprite;

			if(0.0 <= angle && angle < 45.0)
			{
				//Player is to the right
				newBulletX = currentEnemy->bulletSpeed;
				newBulletSprite = sprite::BlueBoltHorizontal;
			}
			else if(45.0 <= angle && angle < 135.0)
			{									
				if(currentEnemy->getYCenter() > player.getYCenter())
				{
					//Player is above
					newBulletY = -currentEnemy->bulletSpeed;
					newBulletSprite = sprite::BlueBoltVert;
				}
				else
				{
					//Player is below
					newBulletY = currentEnemy->bulletSpeed;
					newBulletSprite = sprite::BlueBoltVert;
				}
			}
			else
			{
				//Player is to the left
				newBulletX = -currentEnemy->bulletSpeed;
				newBulletSprite = sprite::BlueBoltHorizontal;
			}

			newBullet = GetNextAvailableEnemyBullet(currentEnemy->enemyID, currentEnemy->maxBullets);
			if(newBullet && (newBulletX != 0.0 || newBulletY != 0.0))
			{
				currentEnemy->lastShotTicks = frameTicks;

				SetupNewBullet(newBullet, 
					currentEnemy->enemyID, 
					currentEnemy->xWorld, currentEnemy->yWorld, newBulletX, newBulletY, 
					frameTicks, newBulletSprite);
				Mix_PlayChannel(-1, lightningSound, 0);
			}
		}

		currentEnemy->lastActionTicks = frameTicks;
	}
}
void MoveEnemyAIDiagonalShot(Enemy* currentEnemy, int currentIndex, const int frameTicks, double& xMove, double& yMove)
{
	//This AI will fire diagonally for extra challenge!
	if(frameTicks - currentEnemy->lastActionTicks > ENEMY_ACTION_TIME)
	{
		double angle = currentEnemy->getAngleTo(player);

		double Random = genrand_real3();
		if(Random < .3333)
		{
			//Consider the angle between the enemy and the player.
			//If the player is more left than up, we want to move left.
			if(!currentEnemy->isAdjacent(player))
			{
				if(0.0 <= angle && angle < 45.0)
				{
					//Player is to the right
					xMove = ENEMY_ACCEL;
				}
				else if(45.0 <= angle && angle < 135.0)
				{									
					if(currentEnemy->getYCenter() > player.getYCenter())
					{
						//Player is above
						yMove = -ENEMY_ACCEL;
					}
					else
					{
						//Player is below
						yMove = ENEMY_ACCEL;
					}
				}
				else
				{
					//Player is to the left
					xMove = -ENEMY_ACCEL;
				}
			}
		}
		else if(Random < .8667)
		{
			Bullet* newBullet = NULL;
			double newBulletX = 0.0;
			double newBulletY = 0.0;
			sprite::SpriteType newBulletSprite;

			if(0.0 <= angle && angle < 30.0)
			{
				//Player is to the right
				newBulletX = currentEnemy->bulletSpeed;
				newBulletSprite = sprite::GoldBoltHorizontal;
			}
			else if(30.0 <= angle && angle < 60.0)
			{		
				//Player is to the right
				newBulletX = currentEnemy->bulletSpeed / sqrt(2.0);

				if(currentEnemy->getYCenter() > player.getYCenter())
				{
					//Player is above
					newBulletY = -currentEnemy->bulletSpeed / sqrt(2.0);
					newBulletSprite = sprite::GoldBoltUpRight;
				}
				else
				{
					//Player is below
					newBulletY = currentEnemy->bulletSpeed / sqrt(2.0);
					newBulletSprite = sprite::GoldBoltDownRight;
				}
			}
			else if(60.0 <= angle && angle < 120.0)
			{									
				if(currentEnemy->getYCenter() > player.getYCenter())
				{
					//Player is above
					newBulletY = -currentEnemy->bulletSpeed;
					newBulletSprite = sprite::GoldBoltVert;
				}
				else
				{
					//Player is below
					newBulletY = currentEnemy->bulletSpeed;
					newBulletSprite = sprite::GoldBoltVert;
				}
			}
			else if(120.0 <= angle && angle < 150.0)
			{		
				//Player is to the left
				newBulletX = -currentEnemy->bulletSpeed / sqrt(2.0);

				if(currentEnemy->getYCenter() > player.getYCenter())
				{
					//Player is above
					newBulletY = -currentEnemy->bulletSpeed / sqrt(2.0);
					newBulletSprite = sprite::GoldBoltDownRight;
				}
				else
				{
					//Player is below
					newBulletY = currentEnemy->bulletSpeed / sqrt(2.0);
					newBulletSprite = sprite::GoldBoltUpRight;
				}
			}
			else
			{
				//Player is to the left
				newBulletX = -currentEnemy->bulletSpeed;
				newBulletSprite = sprite::GoldBoltHorizontal;
			}

			newBullet = GetNextAvailableEnemyBullet(currentEnemy->enemyID, currentEnemy->maxBullets);
			if(newBullet && (newBulletX != 0.0 || newBulletY != 0.0))
			{
				currentEnemy->lastShotTicks = frameTicks;

				SetupNewBullet(newBullet, 
					currentEnemy->enemyID, 
					currentEnemy->xWorld, currentEnemy->yWorld, newBulletX, newBulletY, 
					frameTicks, newBulletSprite);
				Mix_PlayChannel(-1, lightningSound, 0);
			}
		}

		currentEnemy->lastActionTicks = frameTicks;
	}
}
void MoveEnemyAIDumb(Enemy* currentEnemy, int currentIndex, const int frameTicks, double& xMove, double& yMove)
{
	//Decide on an action
	//1: Approach or retreat from player
	//2: Attack player
	//3: Do nothing
	if(frameTicks - currentEnemy->lastActionTicks > ENEMY_ACTION_TIME)
	{
		double angle = currentEnemy->getAngleTo(player);

		double Random = genrand_real3();
		if(Random < .3333)
		{
			//Consider the angle between the enemy and the player.
			//If the player is more left than up, we want to move left.
			if(!currentEnemy->isAdjacent(player))
			{
				if(0.0 <= angle && angle < 45.0)
				{
					//Player is to the right
					xMove = ENEMY_ACCEL;
				}
				else if(45.0 <= angle && angle < 135.0)
				{									
					if(currentEnemy->getYCenter() > player.getYCenter())
					{
						//Player is above
						yMove = -ENEMY_ACCEL;
					}
					else
					{
						//Player is below
						yMove = ENEMY_ACCEL;
					}
				}
				else
				{
					//Player is to the left
					xMove = -ENEMY_ACCEL;
				}

				//There is a half-chance of the enemy going the opposite direction
				//to spice things up.
				if(Random < .1666)
				{
					xMove = -xMove;
					yMove = -yMove;
				}
			}
		}
		else if(Random < .6667)
		{
			Bullet* newBullet = NULL;
			double newBulletX = 0.0;
			double newBulletY = 0.0;
			sprite::SpriteType newBulletSprite;

			if(0.0 <= angle && angle < 45.0)
			{
				//Player is to the right
				newBulletX = currentEnemy->bulletSpeed;
				newBulletSprite = sprite::GreenBoltHorizontal;
			}
			else if(45.0 <= angle && angle < 135.0)
			{									
				if(currentEnemy->getYCenter() > player.getYCenter())
				{
					//Player is above
					newBulletY = -currentEnemy->bulletSpeed;
					newBulletSprite = sprite::GreenBoltVert;
				}
				else
				{
					//Player is below
					newBulletY = currentEnemy->bulletSpeed;
					newBulletSprite = sprite::GreenBoltVert;
				}
			}
			else
			{
				//Player is to the left
				newBulletX = -currentEnemy->bulletSpeed;
				newBulletSprite = sprite::GreenBoltHorizontal;
			}

			newBullet = GetNextAvailableEnemyBullet(currentEnemy->enemyID, currentEnemy->maxBullets);
			if(newBullet && (newBulletX != 0.0 || newBulletY != 0.0))
			{
				currentEnemy->lastShotTicks = frameTicks;

				SetupNewBullet(newBullet, 
					currentEnemy->enemyID, 
					currentEnemy->xWorld, currentEnemy->yWorld, newBulletX, newBulletY, 
					frameTicks, newBulletSprite);
				Mix_PlayChannel(-1, lightningSound, 0);
			}
		}

		currentEnemy->lastActionTicks = frameTicks;
	}
}
void MoveEnemy(const int frameTicks)
{
	//Do nothing for a second
	if((frameTicks - levelStartTicks) > 1000)
	{
		Enemy* currentEnemy;
		for(int i = 0; i < MAX_ENEMIES; i++)
		{
			currentEnemy = &enemy[i];
			if(currentEnemy->isAlive)
			{
				//Has this enemy been hit by a bullet?
				Bullet* currentBullet;
				for(int j = 0; j < MAX_PLAYER_BULLET; j++)
				{
					currentBullet = &playerBullets[j];
					if(currentBullet->isAlive)
					{
						if(currentBullet->isOverlapping(*currentEnemy))
						{
							currentBullet->isAlive = false;

							if(currentEnemy->hitPoints > 1)
							{
								currentEnemy->lastHitTicks = frameTicks;
								currentEnemy->hitPoints -= 1;
								Mix_PlayChannel(-1, whackSound, 0);
							}
							else
							{
								currentEnemy->isAlive = false;
								Mix_PlayChannel(-1, splatSound, 0);
							}
						}
					}
				}

				if(currentEnemy->isAlive)
				{
					//Check and see if the player has moved into this square.			
					if(currentEnemy->isOverlapping(player))
					{
						//Reject him in the direction opposite its velocity.
						if(player.xVel != 0.0 || player.yVel != 0.0)
						{
							MoveObject(player, -player.xVel, -player.yVel);
						}
					}

					double xMove, yMove;
					xMove = yMove = 0.0;

					switch(currentEnemy->aiType)
					{
					case constants::TriggerHappy:
						MoveEnemyAITriggerHappy(currentEnemy, i, frameTicks, xMove, yMove);
					case constants::DiagonalShot:
						MoveEnemyAIDiagonalShot(currentEnemy, i, frameTicks, xMove, yMove);
					default:
						MoveEnemyAIDumb(currentEnemy, i, frameTicks, xMove, yMove);
					}					

					if(xMove != 0.0 || yMove != 0.0)
					{
						MoveObject(*currentEnemy, xMove, yMove);				
						
						//Don't walk on other enemies
						Enemy* otherEnemy;
						for(int k = 0; k < MAX_ENEMIES; k++)
						{
							if(i != k)
							{
								otherEnemy = &enemy[k];
								if(otherEnemy->isAlive)
								{
									if(otherEnemy->isOverlapping(*currentEnemy))
									{
										currentEnemy->xWorld -= xMove;
										currentEnemy->yWorld -= yMove;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
void EnemyActions(const int frameTicks)
{
	MoveEnemy(frameTicks);
	MoveEnemyBullets(frameTicks);
}

void InitializeLevel(int levelID)
{
	//Get the game level information for the currentLevel
	Level* newLevel;
	newLevel = &gameLevels.levels[levelID];

	//Change the music
	if(newLevel->musicID != (int)musicType)
	{
		Mix_FadeOutMusic(100);
		musicType = (constants::MusicType)newLevel->musicID;
	}

	//Set up the player
	player.xWorld = newLevel->levelPlayerInfo.xWorld;
	player.yWorld = newLevel->levelPlayerInfo.yWorld;
	player.xVel = player.yVel = player.xAccel = player.yAccel = 0.0;

	//Clear bullets
	Bullet* currentBullet;
	for(int i = 0; i < MAX_PLAYER_BULLET; i++)
	{
		currentBullet = &playerBullets[i];
		if(currentBullet->isAlive)
		{
			currentBullet->isAlive = false;
		}
	}
	for(int i = 0; i < MAX_ENEMY_BULLET; i++)
	{
		currentBullet = &enemyBullets[i];
		if(currentBullet->isAlive)
		{
			currentBullet->isAlive = false;
		}
	}

	//Set up the enemies
	Enemy* currentEnemy;
	LevelEnemyInfo* currentEnemyInfo;
	for(int i = 0; i < newLevel->enemyCount; i++)
	{
		currentEnemy = &enemy[i];
		currentEnemyInfo = &newLevel->levelEnemyInfo[i];

		currentEnemy->enemyID = i;
		currentEnemy->maxBullets = currentEnemyInfo->maxBullets;
		currentEnemy->aiType = (constants::AIType)currentEnemyInfo->aiType;
		currentEnemy->bulletSpeed = currentEnemyInfo->bulletSpeed;

		currentEnemy->lastActionTicks = 0;
		currentEnemy->lastShotTicks = 0;
		currentEnemy->lastHitTicks = 0;

		currentEnemy->isAlive = true;
		currentEnemy->xWorld = currentEnemyInfo->xWorld;
		currentEnemy->yWorld = currentEnemyInfo->yWorld;
		currentEnemy->spriteType = (sprite::SpriteType)currentEnemyInfo->spriteID;
		currentEnemy->hitPoints = currentEnemyInfo->hitPoints;
		currentEnemy->xVel = currentEnemy->yVel = currentEnemy->xAccel = currentEnemy->yAccel = 0.0;
	}
}

bool IsEnemyAlive()
{
	//Check for any living enemies
	for(int i = 0; i < gameLevels.levels[currentLevel - 1].enemyCount; i++)
	{
		if(enemy[i].isAlive)
			return true;
	}
	return false;
}

void GameActions(const int frameTicks)
{
	//Start playing the music if it isn't already started
	if(Mix_PlayingMusic() == 0)
	{
		Mix_FadeInMusic(allMusic[(int)musicType], -1, 200);
	}
	if(currentLevel == 0)
	{
		if(playingMode == constants::Waiting)
		{
			//Start the game
			currentLevel = 1;
			levelStartTicks = 0;
			levelEndTicks = 0;
		}
	}
	else if(currentLevel <= gameLevels.levelsCount)
	{		
		if(levelStartTicks == 0)
		{
			//Initialize the level
			playingMode = constants::Playing;
			levelStartTicks = frameTicks;
			levelEndTicks = 0;
			InitializeLevel(currentLevel - 1);
		}
		if(playingMode == constants::Playing)
		{
			if(!player.isAlive)
			{
				//Queue the death music
				if(Mix_PlayingMusic() == 1)
				{
					Mix_FadeOutMusic(30);
				}
				musicType = constants::Death;

				//Game over when the player is killed
				playingMode = constants::GameOver;
				levelEndTicks = frameTicks;
			}
			if(!IsEnemyAlive())
			{
				//End the level when the last enemy is killed
				playingMode = constants::EndOfLevel;
				levelEndTicks = frameTicks;
			}
		}
		else if(playingMode == constants::GameOver)
		{
			//Wait a couple seconds after game over to reset
			if (frameTicks - levelEndTicks > 5000)
			{
				ResetGame();
			}
		}
		if(playingMode == constants::EndOfLevel && frameTicks - levelEndTicks > 2000)
		{
			//Pause before starting the next level
			currentLevel++;
			levelStartTicks = 0;
		}
	}
	else
	{
		if(playingMode != constants::GameCompleted)
		{
			//Queue the victory music!
			if(Mix_PlayingMusic() == 1)
			{
				Mix_FadeOutMusic(30);
			}
			musicType = constants::Victory;

			//When all the levels are done, the game is over
			playingMode = constants::GameCompleted;
			levelStartTicks = 0;
			levelEndTicks = frameTicks;
		}
		else if (frameTicks - levelEndTicks > 5000)
		{
			//Reset the game after a few seconds
			ResetGame();
		}
	}
}

void Actions(const int frameTicks)
{
	GameActions(frameTicks);
	if(playingMode == constants::Playing)
	{
		PlayerActions(frameTicks);
		EnemyActions(frameTicks);
	}
}
void DrawCamera()
{
	//Only want to draw the map that would appear in the camera's view.
	//This game has an unmovable camera.
	int firstRow, firstColumn;
	firstColumn = (int)camera.xWorld;
	firstRow = (int)camera.yWorld;

	int lastRow, lastColumn;
	lastColumn = (int)(camera.getRight() / sprite::TILEMAPSIZE);
	lastRow = (int)(camera.getBottom() / sprite::TILEMAPSIZE);

	for(int i = firstRow; i < lastRow; i++)
	{
		//Get the row of sprites
		int* sprites = currentMap.getSprites(i);
		for(int j = firstColumn; j < lastColumn; j++)
		{
			//Apply the sprite to the position
			sprite::SpriteType spriteType = (sprite::SpriteType)sprites[j];
			ApplySprite(spriteType, tileMap, 
				camera.xScreenOrigin + (j - firstColumn) * sprite::TILEMAPSIZE, 
				camera.yScreenOrigin + (i - firstRow) * sprite::TILEMAPSIZE, screen
				);
		}
	}

	//Draw the player sprite
	if(player.isAlive)
	{
		ApplySprite(player.spriteType, tileMap, (int)player.xWorld, (int)player.yWorld, screen);
	}

	//Draw the enemy sprites
	Enemy* currentEnemy;
	for(int i = 0; i < MAX_ENEMIES; i++)
	{
		currentEnemy = &enemy[i];
		if(currentEnemy->isAlive)
		{
			ApplySprite(currentEnemy->spriteType, tileMap, (int)currentEnemy->xWorld, (int)currentEnemy->yWorld, screen);
		}
	}

	//Draw player bullets
	Bullet* currentBullet;
	for(int i = 0; i < MAX_PLAYER_BULLET; i++)
	{
		currentBullet = &playerBullets[i];
		if(currentBullet->isAlive)
		{
			ApplySprite(currentBullet->spriteType, tileMap, (int)currentBullet->xWorld, (int)currentBullet->yWorld, screen);
		}
	}

	//Draw enemy bullets
	for(int i = 0; i < MAX_ENEMY_BULLET; i++)
	{
		currentBullet = &enemyBullets[i];
		if(currentBullet->isAlive)
		{
			ApplySprite(currentBullet->spriteType, tileMap, (int)currentBullet->xWorld, (int)currentBullet->yWorld, screen);
		}
	}
}
void DrawUI()
{
	//UI starts at camera.getRight, 0.
	//Create temp variables to hold position info
	int x, y, w, h, wx;
	
	//To center text,
	//Place it at (ScreenWidth + camera.GetRight - TextWidth) / 2.
	//This is seen by offsetting an equal distance to the right of the camera
	//as from the right edge of the screen
	TTF_SizeText(titleFont, "LARP Invaders", &w, &h);
	wx = (int)((SCREEN_WIDTH + camera.getRight() - w) / 2);
	ApplyText(titleFont, "LARP Invaders", uiTextColor, wx, 0, screen, NULL);

	x = (int)camera.getRight() + 5;
	y = h + 5;

	std::stringstream health;
	health << "HP: " << player.hitPoints;

	TTF_SizeText(textFont, health.str().c_str(), &w, &h);
	ApplyText(textFont, health.str().c_str(), uiTextColor, x, y, screen, NULL);
	y += h + 5;

	if(playingMode != constants::GameCompleted && playingMode != constants::Menu)
	{
		std::stringstream level;
		level << "Level: " << currentLevel;	

		TTF_SizeText(textFont, level.str().c_str(), &w, &h);
		ApplyText(textFont, level.str().c_str(), uiTextColor, x, y, screen, NULL);
		y += h + 5;
	}

	if(playingMode == constants::EndOfLevel)
	{
		std::stringstream levelComplete;
		levelComplete << "Level complete!";

		TTF_SizeText(textFont, levelComplete.str().c_str(), &w, &h);
		ApplyText(textFont, levelComplete.str().c_str(), uiTextColor, x, y, screen, NULL);
		y += h + 5;
	}
	else if(playingMode == constants::GameOver)
	{
		std::stringstream gameOver;
		gameOver << "Game over!";

		TTF_SizeText(textFont, gameOver.str().c_str(), &w, &h);
		ApplyText(textFont, gameOver.str().c_str(), uiTextColor, x, y, screen, NULL);
		y += h + 5;
	}
	else if(playingMode == constants::GameCompleted)
	{
		std::stringstream gameComplete;
		gameComplete << "Game complete!";

		TTF_SizeText(textFont, gameComplete.str().c_str(), &w, &h);
		ApplyText(textFont, gameComplete.str().c_str(), uiTextColor, x, y, screen, NULL);
		y += h + 5;
	}
	else if(playingMode == constants::Menu)
	{
		std::stringstream menuText;
		menuText << "Press enter to play!";

		TTF_SizeText(textFont, menuText.str().c_str(), &w, &h);
		ApplyText(textFont, menuText.str().c_str(), uiTextColor, x, y, screen, NULL);
		y += h + 5;
	}
}

int GameLoop()
{
	Timer Timer;
	int Frame = 0;

	bool quit = false;
	SDL_Event PollEvent;
	while(!quit)
	{
		int frameTicks = Timer.start();

		while(SDL_PollEvent(&PollEvent))
		{
			if(PollEvent.type == SDL_QUIT)
			{
				quit = true;
				break;
			}
			if(HandleInput(PollEvent, frameTicks) == -1)
			{
				quit = true;
				break;
			}			
		}
		if(quit == true)
			break;

		Actions(frameTicks);
		
		SDL_FillRect(screen, &(screen->clip_rect), backgroundColor);

		DrawCamera();
		DrawUI();

		if(SDL_Flip(screen) == -1)
			return 1;

        //Cap the frame rate
        if( Timer.get_ticks() < 1000 / MAX_FPS )
        {
			int delay =  ( 1000 / MAX_FPS ) - Timer.get_ticks();
            SDL_Delay(delay);
        }

		Frame++;
		if(Frame % MAX_FPS == 0)
			Frame = 0;
	}

	return 0;
}

int main(int argc, char *argv[])
{
	int result = 0;
	if(!init())
	{
		result = 1;
		goto exit;
	}

	//Initialize startup music
	musicType = constants::BarnBeat;

	ResetGame();

	result = GameLoop();

exit:
	shutdown();
	return result;
}
