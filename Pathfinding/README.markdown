This project contains the A* path-finding algorithm implemented in F# and showcases it in a simple graphical application. In which a character finds his way around a simple 2D, Tile based, maze.

The main deliverable here is the A* algorithm as implemented in F#; the rest of the project is only provided for demonstration purposes.

*Requires*
http://cs-sdl.sourceforge.net/

*Utils.fs*
Contains the basic A* algorithm and some utilities for reading files, manipulating lists, etc

*Pathfinder.fs*
Contains a function that curries the A* algorithm with 2D tilemap specific functions and is used as an example of how to adapt the algorythm to your needs.

The remaining files are just used to showcase the path-finding in a simple graphical application that uses SDL.NET.

*When the demo is running:*
Pressing M causes the cloaked figure to reset to the bottom right corner and then find his way to the chest,
Pressing N causes him to find his way to the bottom left corner... you can interrupt his journey to the chest at any point with this.

Experiment with the level files to define different mazes.

The images used where the result of a google image search and are not my work and are only provided as an example.