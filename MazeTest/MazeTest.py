from enum import IntEnum
from enum import Enum
import random
import pygame
import os
import sys

###########
# DEFINES #
###########

# Determine whether or not you want there to be branches in the maze.
branching = True

# Min and max percent densities of the maze. Based on tests, can be adjusted later very easily.
if not branching:
	minDensity = 0.10
	maxDensity = 0.60
if branching:
	minDensity = 0.50
	maxDensity = 0.90

# Width and height of the overall grid the maze CAN occupy.
width = 20
height = 20

# Starting position of the first lobby.
startX = 8
startY = 8

# Minimum distance the end point can be from the starting point.
minDistance = 6

# The number of possible end points. This is if we decide one end point is too cruel.
numEndPoints = 1

for arg in sys.argv:
	if arg.startswith("branching="):
		if arg.endswith("true") or arg.endswith("True"):
			branching = True
		elif arg.endswith("false") or arg.endswith("False"):
			branching = False
	elif arg.startswith("minDensity="):
		minDensity = float(arg[11:])
	elif arg.startswith("maxDensity="):
		maxDensity = float(arg[11:])
#	elif arg.startswith("width="):		# For some reason, these commands break it....
#		width = int(arg[6:])
#	elif arg.startswith("height="):
#		height = int(arg[7:])
	elif arg.startswith("startX="):
		startX = int(arg[7:])
	elif arg.startswith("startY="):
		startY = int(arg[7:])
	elif arg.startswith("minDistance="):
		minDistance = int(arg[12:])
	elif arg.startswith("numEndPoints="):
		numEndPoints = int(arg[13:])

###############
# END DEFINES #
###############

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
	TWSE = 8	# T shape running West, South, and East
	TWNE = 9
	TNES = 10
	TNWS = 11
	START = 12
	END = 13

class Dir(Enum):
	N = 0
	S = 1
	E = 2
	W = 3

class MazeBuilder:

	def __init__(self):
		print("Width: " + str(width) + "  Height: " + str(height))
		self.maze = [[Tile.WALL for i in range(width)] for i in range(height)]
		self.tileImgs = []
		self.screen = None
		self.numTiles = 0

		area = width * height
		self.minTiles = minDensity * area
		self.maxTiles = maxDensity * area
		self.lobbies = []

	def isLegal(self, x, y):
		if x >= width or y >= height:
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

	def getTiles(self, ndir):
		tiles = [Tile.LOBBY, Tile.CNE, Tile.CNW, Tile.CSE, Tile.CSW, Tile.HNS, Tile.HEW, Tile.TWSE, Tile.TWNE, Tile.TNES, Tile.TNWS]

		if ndir == Dir.N:
			tiles.remove(Tile.CNE)
			tiles.remove(Tile.CNW)
			tiles.remove(Tile.HEW)
			tiles.remove(Tile.TWNE)
		elif ndir == Dir.S:
			tiles.remove(Tile.CSE)
			tiles.remove(Tile.CSW)
			tiles.remove(Tile.HEW)
			tiles.remove(Tile.TWSE)
		elif ndir == Dir.E:
			tiles.remove(Tile.CNE)
			tiles.remove(Tile.CSE)
			tiles.remove(Tile.HNS)
			tiles.remove(Tile.TNES)
		elif ndir == Dir.W:
			tiles.remove(Tile.CNW)
			tiles.remove(Tile.CSW)
			tiles.remove(Tile.HNS)
			tiles.remove(Tile.TNWS)

		return tiles
		
	def curDistance(self, curX, curY):
		x = abs(startX - curX)
		y = abs(startY - curY)
		return x + y

	def buildMaze(self, x, y, tile, ldir):

		c = x
		r = y
		cn = -1
		rn = -1
		ndir = None

		self.maze[c][r] = tile
		self.numTiles += 1

		# if self.stepDisplay(c, r, tile):
		# 	return True

		if tile == Tile.LOBBY or tile == Tile.START:
			if self.curDistance(x, y) >= minDistance:
				self.lobbies.append((x, y))
			
			dirs = [Dir.N, Dir.S, Dir.E, Dir.W]
			random.shuffle(dirs)
			for d in dirs:
				cn, rn = self.getNext(c, r, d)
				if self.isLegal(cn, rn):
					tiles = self.getTiles(d)
					tiles.remove(Tile.LOBBY)
					self.buildMaze(cn, rn, random.choice(tiles), d)
					if not branching:
						break

			return
			
		if tile == Tile.TWSE or tile == Tile.TWNE or tile == Tile.TNES or tile == Tile.TNWS:
			dirs = [Dir.N, Dir.W, Dir.S, Dir.E]
			random.shuffle(dirs)
			
			if ldir == Dir.E or tile == Tile.TNES:
				dirs.remove(Dir.W)
			if ldir == Dir.N or tile == Tile.TWNE:
				dirs.remove(Dir.S)
			if ldir == Dir.W or tile == Tile.TNWS:
				dirs.remove(Dir.E)
			if ldir == Dir.S or tile == Tile.TWSE:
				dirs.remove(Dir.N)
				
			for d in dirs:
				cn, rn = self.getNext(c, r, d)
				if self.isLegal(cn, rn):
					tiles = self.getTiles(d)
					self.buildMaze(cn, rn, random.choice(tiles), d)
					if not branching:
						break
		
		elif tile == Tile.CNE:
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

		tiles = self.getTiles(ndir)
			
		cn, rn = self.getNext(c, r, ndir)
		if self.isLegal(cn, rn):
			self.buildMaze(cn, rn, random.choice(tiles), ndir)

		return

	def initDisplay(self):
		self.tileImgs = []

		imgDir = "./MazeImages"

		for s, d, files in os.walk(imgDir):
			for f in files:
				self.tileImgs.append(pygame.image.load(os.path.join(imgDir, f)))

		self.screen = pygame.display.set_mode((width * 20, height * 20))

	def stepDisplay(self, x, y, tile):
		xc = x * 20
		yc = y * 20

		while(True):
			evt = pygame.event.wait()
			if pygame.key.get_pressed()[pygame.K_SPACE]:
				break
			if evt.type == pygame.QUIT:
				pygame.display.quit()
				return True

		self.screen.blit(self.tileImgs[int(tile)], (xc, yc))
		pygame.display.flip()


	def checkMaze(self):
		# Checks the number of tiles in the maze. If inappropriate, or if there is no eligible lobby for an end point, rebuild the maze.
		print(self.numTiles)

		if self.numTiles < self.minTiles or self.numTiles > self.maxTiles or len(self.lobbies) < numEndPoints:
			self.lobbies = []
			self.numTiles = 0
			self.maze = [[Tile.WALL for i in range(width)] for i in range(height)]
			self.buildMaze(startX, startY, Tile.START, Dir.W)
			return False

		return True
		
	def pickEndPoints(self):
		# Assume self.lobbies is NOT empty; this method is called AFTER checkMaze, which assures this
		random.shuffle(self.lobbies)
		for i in range(len(self.lobbies)):
			c = self.lobbies[i][0]
			r = self.lobbies[i][1]
			self.maze[c][r] = Tile.END
			if i >= numEndPoints - 1:
				break

	def displayMaze(self):
		x = 0
		y = 0

		for i in self.maze:
			for j in i:
				self.screen.blit(self.tileImgs[int(j)], (x, y))
				y += 20
				
			#print(i)
			x += 20
			y = 0
			
		pygame.display.flip()
###################################################################
# END CLASS DECLARATION

maze = MazeBuilder()

maze.initDisplay()

maze.buildMaze(startX, startY, Tile.START, Dir.W)

while not maze.checkMaze():
	continue
	
maze.pickEndPoints()

maze.displayMaze()
print ("Lobbies: " + str(len(maze.lobbies)))

done = False
while(not done):
	evt = pygame.event.wait()
	if evt.type == pygame.QUIT:
		pygame.display.quit()
		done = True