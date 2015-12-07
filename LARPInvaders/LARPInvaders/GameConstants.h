#ifndef GAMECONSTANTS_H
#define GAMECONSTANTS_H

#include "SpriteType.h"

const int MUSIC_TYPES = 5;

const int MAX_LEVELS = 99;

const int MAX_ENEMIES = 10;

const int MAX_PLAYER_BULLET = 5;
const int MAX_ENEMY_BULLET = 50;

const double PLAYER_ACCEL = sprite::TILEMAPSIZE / 2.0;
const double ENEMY_ACCEL = PLAYER_ACCEL;
const int PLAYER_ACCEL_TIME = 75;
const int ENEMY_ACTION_TIME = 200;

const double PLAYER_BULLET_ACCEL = sprite::TILEMAPSIZE / 4.0;
const double PLAYER_BULLET_TIME = 15;

const double ENEMY_BULLET_ACCEL = sprite::TILEMAPSIZE / 4.0;
const double ENEMY_BULLET_TIME = 15;

namespace constants
{

	enum PlayingMode
	{
		Waiting,
		Playing,
		EndOfLevel,
		GameOver,
		GameCompleted,
		Menu
	};

	enum MusicType
	{
		BarnBeat,
		LegendaryVirtue,
		FinalBattle,
		Death,
		Victory
	};

	enum AIType
	{
		Dumb,
		TriggerHappy,
		DiagonalShot
	};

}

#endif