#include "Map.h"
#include "StdReferences.h"
#include "SpriteType.h"
#include "GraphicsUtils.h"

Map::Map()
{		
	rows = 0;
	columns = 0;
	for(int i = 0; i < MAX_MAP_ROWS; i++)
		for(int j = 0; j < MAX_MAP_COLUMNS; j++)
			sprites[i][j] = 0;
}

int Map::getRows() { return rows; }
int Map::getColumns() { return columns; } 
int* Map::getSprites(int row) { return sprites[row]; }

void Map::Load(std::string fileName)
{
	int i = 0;
	int j = 0;
	int value = 0;
	std::string buffer;	
	std::ifstream input(fileName, std::ifstream::in);	

	while(i < MAX_MAP_ROWS && std::getline(input, buffer))
	{
		std::istringstream line(buffer);
		while(j < MAX_MAP_COLUMNS && line >> value)
		{
			sprites[i][j] = value;
			j++;
		}
		i++;
		if(columns == 0)
			columns = j;
		j = 0;
	}
	rows = i;
	input.close();
}

void Map::Draw(SDL_Surface* tileMap, SDL_Surface* screen, int x, int y, int w, int h)
{
	for(int i = 0; i < rows; i++)
	{
		for(int j = 0; j < columns; j++)
		{
			sprite::SpriteType spriteType = (sprite::SpriteType)sprites[i][j];
			ApplySprite(spriteType, tileMap, x + j * sprite::TILEMAPSIZE, y + i * sprite::TILEMAPSIZE, screen);
		}
	}
}