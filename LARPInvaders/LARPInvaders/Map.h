#ifndef MAP_H
#define MAP_H

const int MAX_MAP_ROWS = 30;
const int MAX_MAP_COLUMNS = 30;

#include "SDLReferences.h"
#include "StdReferences.h"

class Map
{

public:
	Map();

	void Load(std::string fileName);
	void Draw(SDL_Surface* tileMap, SDL_Surface* screen, int x, int y, int w, int h);

	int getRows();
	int getColumns();
	int* getSprites(int row);	

private:
	int sprites[MAX_MAP_ROWS][MAX_MAP_COLUMNS];
	int rows;
	int columns;

};

#endif