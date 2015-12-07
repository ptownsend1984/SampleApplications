#ifndef SPRITETYPE_H
#define SPRITETYPE_H

namespace sprite {

const int TILEMAPSIZE = 32;

								//Row,Column
enum SpriteType
{
	GrassDark,					//52,21
	GrassMedium,				//52,20
	GrassLight,					//52,22
	WaterDark,					//52,24
	WaterMedium,				//52,23
	WaterLight,					//52,25
	TreeSmall,					//2,31
	TreeTall,					//2,28
	GravelDark,					//0,0
	GravelLightGray,			//0,1
	GravelDarkGray,				//0,2
	GravelLight,				//0,3
	StoneWallDark,				//0,5
	StoneWallGray,				//0,4
	StoneWallLight,				//0,6
	StoneBrickDarkGray,			//0,20
	StoneBrickLightGray,		//0,19
	StoneBrickLight,			//0,21
	StoneDoorClosed,			//2,3
	StoneDoorOpen,				//2,4
	StoneDoorBroken,			//2,5

	Fighter,					//42, 18
	RatWhite,					//44, 5
	RatGray,					//44, 6

	DragonBlue,					//38, 4
	DragonGreen,				//38, 6

	Orc,						//40, 25
	Drow,						//48, 20
	Skeleton,					//44, 11

	RedBoltVert,				//15, 0
	RedBoltHorizontal,			//15, 1
	RedBoltUpRight,				//15, 2
	RedBoltDownRight,			//15, 3

	BlueBoltVert,				//15, 4
	BlueBoltHorizontal,			//15, 5
	BlueBoltUpRight,			//15, 6
	BlueBoltDownRight,			//15, 7

	CyanBoltVert,				//15, 8
	CyanBoltHorizontal,			//15, 9
	CyanBoltUpRight,			//15, 10
	CyanBoltDownRight,			//15, 11

	GreenBoltVert,				//15, 12
	GreenBoltHorizontal,		//15, 13
	GreenBoltUpRight,			//15, 14
	GreenBoltDownRight,			//15, 15

	WhiteBoltVert,				//15, 16
	WhiteBoltHorizontal,		//15, 17
	WhiteBoltUpRight,			//15, 18
	WhiteBoltDownRight,			//15, 19

	PurpleBoltVert,				//15, 20
	PurpleBoltHorizontal,		//15, 21
	PurpleBoltUpRight,			//15, 22
	PurpleBoltDownRight,		//15, 23

	GoldBoltVert,				//15, 24
	GoldBoltHorizontal,			//15, 25
	GoldBoltUpRight,			//15, 26
	GoldBoltDownRight,			//15, 27

	GrayBoltVert,				//15, 28
	GrayBoltHorizontal,			//15, 29
	GrayBoltUpRight,			//15, 30
	GrayBoltDownRight,			//15, 31

};

}
#endif