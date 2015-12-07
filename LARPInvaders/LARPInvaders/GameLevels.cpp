#include "GameLevels.h"

GameLevels::GameLevels()
{
	levelsCount = 0;
}

void GameLevels::Load(std::string fileName)
{
	int i = -1;
	char peek;
	std::string buffer;	
	std::string read;
	std::ifstream input(fileName, std::ifstream::in);	
	int levelLine = 0;

	while(i < MAX_LEVELS && std::getline(input, buffer))
	{		
		//Level format:
		//L	
		//MusicID	
		//PlayerX	PlayerY
		//EnemyX	EnemyY	EnemyAIType	EnemySpriteID	EnemyHP	MaxBullets	BulletSpeed
		//
		//Enemies repeat until next level begins or file ends
		std::istringstream line(buffer);
		peek = line.peek();

		//Use a slash for a comment
		if(peek != '/')
		{
			//Look for the L to signify the next level definition
			if(peek == 'L')
			{
				i++;
				levels[i].ID = i;
				levelLine = 0;
			}
			else
			{
				levelLine++;
				//First line is level info
				//Second line is player info
				//Rest are enemies, removing 3 from the levelLine for the proper index
				if(levelLine == 1)
				{
					line >> levels[i].musicID;
				}
				else if(levelLine == 2)
				{
					line >> levels[i].levelPlayerInfo.xWorld;
					line >> levels[i].levelPlayerInfo.yWorld;
				}
				else
				{
					levels[i].enemyCount++;
					line >> levels[i].levelEnemyInfo[levelLine-3].xWorld;
					line >> levels[i].levelEnemyInfo[levelLine-3].yWorld;
					line >> levels[i].levelEnemyInfo[levelLine-3].aiType;
					line >> levels[i].levelEnemyInfo[levelLine-3].spriteID;
					line >> levels[i].levelEnemyInfo[levelLine-3].hitPoints;
					line >> levels[i].levelEnemyInfo[levelLine-3].maxBullets;
					line >> levels[i].levelEnemyInfo[levelLine-3].bulletSpeed;
				}
			}		
		}
	}
	//levelsCount is 1 more than the iterator
	levelsCount = i + 1;
	input.close();
}
