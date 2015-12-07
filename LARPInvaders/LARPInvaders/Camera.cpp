#include "Camera.h"

Camera::Camera()
{	
	ResetView();
}

void Camera::ResetView()
{	
	xWorld = yWorld = 0.0;
	xScreenOrigin = yScreenOrigin = 0;
	wWorld = CAMERA_WIDTH;
	hWorld = CAMERA_HEIGHT;
}