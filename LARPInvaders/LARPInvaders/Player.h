#ifndef PLAYER_H
#define PLAYER_H

#include "PhysicalObject.h"

class Player : public PhysicalObject
{

public:
	Player();

	//Key down time for horizontal/vertical movement
	int keyDownTicks[4];

};

#endif