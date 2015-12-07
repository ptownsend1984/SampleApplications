#include "Master.h"

inline bool operator == (SDL_Color a, SDL_Color b) { return a.r == b.r && a.g == b.g && a.b == b.b; }
inline bool operator != (SDL_Color a, SDL_Color b) { return !(a == b); }

inline int SDL_MapRGB(SDL_PixelFormat* format, SDL_Color& color) { return SDL_MapRGB(format, color.r, color.g, color.b); }
inline int SDL_FillRect(SDL_Surface* destination, SDL_Rect* r, SDL_Color& color) { return SDL_FillRect(destination, r, SDL_MapRGB(destination->format, color)); }

SDL_Color CreateColor(Uint8 a, Uint8 r, Uint8 g, Uint8 b)
{
	SDL_Color c;
	c.unused = a;
	c.r = r;
	c.g = g;
	c.b = b;
	return c;
}
SDL_Color CreateColor(Uint8 r, Uint8 g, Uint8 b)
{
	return CreateColor(0xFF, r, g, b);
}

SDL_Rect CreateRect(Sint16 x, Sint16 y, Uint16 w, Uint16 h)
{
	SDL_Rect r;
	r.h = h;
	r.w = w;
	r.x = x;
	r.y = y;
	return r;
}

SDL_Surface* LoadImage(std::string filename, SDL_Color& MapColor)
{
	SDL_Surface* LoadedImage = NULL;
	SDL_Surface* OptimizedImage = NULL;

	LoadedImage = IMG_Load(filename.c_str());

	if(LoadedImage)
	{
		OptimizedImage = SDL_DisplayFormat(LoadedImage);
		SDL_FreeSurface(LoadedImage);
	}
	if(OptimizedImage)
	{
		Uint32 colorKey = SDL_MapRGB(OptimizedImage->format, MapColor);
		SDL_SetColorKey(OptimizedImage, SDL_SRCCOLORKEY, colorKey);
	}
	return OptimizedImage;
}

void ApplySurface(int x, int y, SDL_Surface* source, SDL_Surface* destination, SDL_Rect* clip)
{
	SDL_Rect offset;
	offset.x = x;
	offset.y = y;
	SDL_BlitSurface(source, clip, destination, &offset);
}

bool ApplyText(TTF_Font* font, const char* message, SDL_Color color, int x, int y, SDL_Surface* destination, SDL_Rect* clip)
{
	SDL_Surface* Message = TTF_RenderText_Solid(font, message, color);
	if(!Message)	
		return false;

	ApplySurface(x, y, Message, destination, clip);
	SDL_FreeSurface(Message);

	return true;
}

void ApplySprite(sprite::SpriteType spriteType, SDL_Surface* tileMap, int x, int y, SDL_Surface* destination)
{
	int row, column;
	row = column = 0;
	switch(spriteType)
	{
	case sprite::GrassDark:
		row = 52; 
		column = 21;
		break;
	case sprite::GrassMedium:
		row = 52; 
		column = 20;
		break;
	case sprite::GrassLight:
		row = 52; 
		column = 22;
		break;
	case sprite::WaterDark:
		row = 52; 
		column = 24;
		break;
	case sprite::WaterMedium:
		row = 52; 
		column = 23;
		break;
	case sprite::WaterLight:
		row = 52; 
		column = 25;
		break;
	case sprite::TreeSmall:
		row = 2; 
		column = 31;
		break;
	case sprite::TreeTall:
		row = 2; 
		column = 28;
		break;
	case sprite::GravelDark:
		row = 0; 
		column = 0;
		break;
	case sprite::GravelLightGray:
		row = 0; 
		column = 1;
		break;
	case sprite::GravelDarkGray:
		row = 0; 
		column = 2;
		break;
	case sprite::GravelLight:
		row = 0; 
		column = 3;
		break;
	case sprite::StoneWallDark:
		row = 0; 
		column = 5;
		break;
	case sprite::StoneWallGray:
		row = 0; 
		column = 4;
		break;
	case sprite::StoneWallLight:
		row = 0; 
		column = 6;
		break;
	case sprite::StoneBrickDarkGray:
		row = 0; 
		column = 20;
		break;
	case sprite::StoneBrickLightGray:
		row = 0; 
		column = 19;
		break;
	case sprite::StoneBrickLight:
		row = 0; 
		column = 21;
		break;
	case sprite::StoneDoorClosed:
		row = 2; 
		column = 3;
		break;
	case sprite::StoneDoorOpen:
		row = 2; 
		column = 4;
		break;
	case sprite::StoneDoorBroken:
		row = 2; 
		column = 5;
		break;
	case sprite::Fighter:
		row = 42; 
		column = 18;
		break;
	case sprite::RatWhite:
		row = 44; 
		column = 5;
		break;
	case sprite::RatGray:
		row = 44; 
		column = 6;
		break;
	case sprite::RedBoltVert:
		row = 15; 
		column = 0;
		break;
	case sprite::RedBoltHorizontal:
		row = 15; 
		column = 1;
		break;
	case sprite::RedBoltUpRight:
		row = 15; 
		column = 2;
		break;
	case sprite::RedBoltDownRight:
		row = 15; 
		column = 3;
		break;
	case sprite::BlueBoltVert:
		row = 15; 
		column = 4;
		break;
	case sprite::BlueBoltHorizontal:
		row = 15; 
		column = 5;
		break;
	case sprite::BlueBoltUpRight:
		row = 15; 
		column = 6;
		break;
	case sprite::BlueBoltDownRight:
		row = 15; 
		column = 7;
		break;
	case sprite::CyanBoltVert:
		row = 15; 
		column = 8;
		break;
	case sprite::CyanBoltHorizontal:
		row = 15; 
		column = 9;
		break;
	case sprite::CyanBoltUpRight:
		row = 15; 
		column = 10;
		break;
	case sprite::CyanBoltDownRight:
		row = 15; 
		column = 11;
		break;
	case sprite::GreenBoltVert:
		row = 15; 
		column = 12;
		break;
	case sprite::GreenBoltHorizontal:
		row = 15; 
		column = 13;
		break;
	case sprite::GreenBoltUpRight:
		row = 15; 
		column = 14;
		break;
	case sprite::GreenBoltDownRight:
		row = 15; 
		column = 15;
		break;
	case sprite::WhiteBoltVert:
		row = 15; 
		column = 16;
		break;
	case sprite::WhiteBoltHorizontal:
		row = 15; 
		column = 17;
		break;
	case sprite::WhiteBoltUpRight:
		row = 15; 
		column = 18;
		break;
	case sprite::WhiteBoltDownRight:
		row = 15; 
		column = 19;
		break;
	case sprite::PurpleBoltVert:
		row = 15; 
		column = 20;
		break;
	case sprite::PurpleBoltHorizontal:
		row = 15; 
		column = 21;
		break;
	case sprite::PurpleBoltUpRight:
		row = 15; 
		column = 22;
		break;
	case sprite::PurpleBoltDownRight:
		row = 15; 
		column = 23;
		break;
	case sprite::GoldBoltVert:
		row = 15; 
		column = 24;
		break;
	case sprite::GoldBoltHorizontal:
		row = 15; 
		column = 25;
		break;
	case sprite::GoldBoltUpRight:
		row = 15; 
		column = 26;
		break;
	case sprite::GoldBoltDownRight:
		row = 15; 
		column = 27;
		break;
	case sprite::GrayBoltVert:
		row = 15; 
		column = 28;
		break;
	case sprite::GrayBoltHorizontal:
		row = 15; 
		column = 29;
		break;
	case sprite::GrayBoltUpRight:
		row = 15; 
		column = 30;
		break;
	case sprite::GrayBoltDownRight:
		row = 15; 
		column = 31;
		break;
	case sprite::DragonBlue:
		row = 38; 
		column = 4;
		break;
	case sprite::DragonGreen:
		row = 38; 
		column = 6;
		break;
	case sprite::Orc:
		row = 40; 
		column = 25;
		break;
	case sprite::Drow:
		row = 48; 
		column = 20;
		break;
	case sprite::Skeleton:
		row = 44; 
		column = 11;
		break;
	//case sprite:::
	//	row = ; 
	//	column = ;
	//	break;
	//case sprite:::
	//	row = ; 
	//	column = ;
	//	break;
	//case sprite:::
	//	row = ; 
	//	column = ;
	//	break;
	//case sprite:::
	//	row = ; 
	//	column = ;
	//	break;
	}
	ApplySprite(row, column, sprite::TILEMAPSIZE, tileMap, x, y, destination);
}
void ApplySprite(int row, int column, int tileMapSize, SDL_Surface* tileMap, int x, int y, SDL_Surface* destination)
{
	ApplySurface(x, y, tileMap, destination, &CreateRect(column * tileMapSize, row * tileMapSize, tileMapSize, tileMapSize));
}
