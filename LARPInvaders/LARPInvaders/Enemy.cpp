#include "Enemy.h"

Enemy::Enemy()
{
	spriteType = sprite::RatGray;
	spawnTicks = 0;
	lastActionTicks = lastShotTicks = 0;
	enemyID = 0;
	maxBullets = 0;
	aiType = constants::Dumb;
	bulletSpeed = 0.0;
}