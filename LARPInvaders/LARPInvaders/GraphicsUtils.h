#ifndef GRAPHICSUTILS_H
#define GRAPHICSUTILS_H

#include "StdReferences.h"
#include "SpriteType.h"

extern inline bool operator == (SDL_Color a, SDL_Color b);
extern inline bool operator != (SDL_Color a, SDL_Color b);

extern inline int SDL_MapRGB(SDL_PixelFormat* format, SDL_Color& color);
extern inline int SDL_FillRect(SDL_Surface* destination, SDL_Rect* r, SDL_Color& color);

extern SDL_Color CreateColor(Uint8 a, Uint8 r, Uint8 g, Uint8 b);
extern SDL_Color CreateColor(Uint8 r, Uint8 g, Uint8 b);

extern SDL_Surface* LoadImage(std::string filename, SDL_Color& MapColor);

extern void ApplySurface(int x, int y, SDL_Surface* source, SDL_Surface* destination, SDL_Rect* clip);

extern bool ApplyText(TTF_Font* font, const char* message, SDL_Color color, int x, int y, SDL_Surface* destination, SDL_Rect* clip);

extern void ApplySprite(sprite::SpriteType spriteType, SDL_Surface* tileMap, int x, int y, SDL_Surface* destination);
extern void ApplySprite(int row, int column, int tileMapSize, SDL_Surface* tileMap, int x, int y, SDL_Surface* destination);

extern SDL_Rect CreateRect(Sint16 x, Sint16 y, Uint16 w, Uint16 h);

#endif