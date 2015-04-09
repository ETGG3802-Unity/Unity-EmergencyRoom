from enum import IntEnum
from enum import Enum
import random
import pygame
import os

pygame.init()

class Tile(IntEnum):
	WALL = 0
	LOBBY = 1
	CNE = 2 #North-East corner (hall runs from north edge to east)
	CNW = 3
	CSE = 4
	CSW = 5
	HNS = 6 #hall running from north to south
	HEW = 7

class Dir(Enum):
	N = 0
	S = 1
	E = 2
	W = 3

class MazeBuilder:

	def __init__(self, width, height):
		self.width = width
		self.height = height
		self.maze = [[Tile.WALL for i in range(width)] for i in range(height)]

	def isLegal(self, x, y):
		if x >= self.width or y >= self.height:
			return False

		if x < 0 or y < 0:
			return False

		if self.maze[x][y] != Tile.WALL:
			return False

		return True

	def getNext(self, x, y, d):
		cn = -1 
		rn = -1 #next coordinate

		if d == Dir.N:
			cn = x
			rn = y - 1
		elif d == Dir.S:
			cn = x
			rn = y + 1
		elif d == Dir.E:
			cn = x + 1
			rn = y
		elif d == Dir.W:
			cn = x - 1
			rn = y

		return cn, rn

	def buildMaze(self, x, y, tile, ldir):

		c = x
		r = y
		cn = -1
		rn = -1

		self.maze[c][r] = tile

		#tiles that can be used next
		tiles = [Tile.LOBBY, Tile.CNE, Tile.CNW, Tile.CSE, Tile.CSW, Tile.HNS, Tile.HEW]

		if ldir == Dir.N:
			tiles.remove(Tile.CNE)
			tiles.remove(Tile.CNW)
			tiles.remove(Tile.HEW)
		elif ldir == Dir.S:
			tiles.remove(Tile.CSE)
			tiles.remove(Tile.CSW)
			tiles.remove(Tile.HEW)
		elif ldir == Dir.E:
			tiles.remove(Tile.CNE)
			tiles.remove(Tile.CSE)
			tiles.remove(Tile.HNS)
		elif ldir == Dir.W:
			tiles.remove(Tile.CNW)
			tiles.remove(Tile.CSW)
			tiles.remove(Tile.HNS)

		#print(tile)

		if tile == Tile.LOBBY:
			tiles.remove(Tile.LOBBY)

			dirs = [Dir.N, Dir.S, Dir.E, Dir.W]
			random.shuffle(dirs)
			for d in dirs:
				cn, rn = self.getNext(c, r, d)
				if self.isLegal(cn, rn):
					self.buildMaze(cn, rn, random.choice(tiles), d)

			return

		ndir = None

		if tile == Tile.CNE:
			if ldir == Dir.S:
				ndir = Dir.E
			elif ldir == Dir.W:
				ndir = Dir.N

		elif tile == Tile.CNW:
			if ldir == Dir.S:
				ndir = Dir.W
			elif ldir == Dir.E:
				ndir = Dir.N

		elif tile == Tile.CSE:
			if ldir == Dir.N:
				ndir = Dir.E
			elif ldir == Dir.W:
				ndir = Dir.S

		elif tile == Tile.CSW:
			if ldir == Dir.N:
				ndir = Dir.W
			elif ldir == Dir.E:
				ndir = Dir.S

		elif tile == Tile.HNS or tile == Tile.HEW:
			ndir = ldir
			
		cn, rn = self.getNext(c, r, ndir)
		if self.isLegal(cn, rn):
			self.buildMaze(cn, rn, random.choice(tiles), ndir)

		return

	def displayMaze(self):
		tileImgs = []

		imgDir = "./MazeImages"

		for s, d, files in os.walk(imgDir):
			for f in files:
				tileImgs.append(pygame.image.load(os.path.join(imgDir, f)))


		screen = pygame.display.set_mode((800,600))

		x = 0
		y = 0

		for i in maze.maze:
			for j in i:
				screen.blit(tileImgs[int(j)], (x, y))
				pygame.display.flip()
				x += 20
				
			#print(i)
			y += 20
			x = 0
			

		

		done = False
		while(not done):
			evt = pygame.event.wait()
			if evt.type == pygame.QUIT:
				done = True
w = 20
h = 20

maze = MazeBuilder(w,h)

maze.buildMaze(10, 10, Tile.LOBBY, Dir.W)

maze.displayMaze()

#x = 1.436, y = 1.375, z = .934
