#ifndef CAMERA_H
#define CAMERA_H

#include "WorldObject.h"

//15 tile square camera
const double CAMERA_WIDTH = 480.0;
const double CAMERA_HEIGHT = 480.0;

class Camera : public WorldObject
{

public:
	Camera();

	int xScreenOrigin;
	int yScreenOrigin;

	void ResetView();
};

#endif