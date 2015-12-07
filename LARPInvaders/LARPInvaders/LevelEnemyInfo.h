#ifndef LEVELENEMYINFO_H
#define LEVELENEMYINFO_H

class LevelEnemyInfo
{

public:
	LevelEnemyInfo();

	double xWorld;
	double yWorld;
	int spriteID;
	int hitPoints;
	int maxBullets;
	double bulletSpeed;
	int aiType;

};

#endif