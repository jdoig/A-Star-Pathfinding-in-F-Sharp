*Requires*
http://cs-sdl.sourceforge.net/

*Utils.fs* 
Contains the basic A* algorythm and some utilities for reading files, manipulating lists, etc

*Pathfinder.fs* 
Contains a funciton that curries the A* algorythm with 2D tilemap specific functions and is used as an example of how to use the algorythm. 

The remaining files are just used to showcase the pathfinding in a simple graphical application that uses SDL.NET.

When the demo is running:
Pressing M causes the cloaked figure to reset to the bottom right corner and then find his way to the chest,
Pressing N cuases him to find his way to the bottom left corner... you can interupt his journey to the chest at any point with this.

Experiment with the level files to define different mazes.