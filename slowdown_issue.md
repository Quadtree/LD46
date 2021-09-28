This issue... is just plain weird. There's no other way to describe it.

When the player ship is removed from the scene tree...
* If there are asteroids...
* And we're in HTML5 mode...
* FPS drops to 40-60 (from 165)

Other things I've tried:
* Switching to Godot native physics
* Profiling: Result was that it seems it's spending all its time in Mono code. Unclear where or why
  * Removing scripts from asteroids did nothing
* Interestingly, removing the rigid bodies from the asteroids (and their visuals) and replacing them with spatials also seems to fix it...?
* However, putting their VISUAL back in causes it again
  * So it's not physics...
* Replacing the asteroids' graphics with random MeshInstances does nothing
* Deleting asteroids doesn't trigger it
* Deleting the ship and asteroids simultaneously causes a drop to 120FPS or so
* Deleting all code in the PlayerShip methods did nothing
* Totally cleaning out the PlayerShip (removing all fields too) did nothing

New theory: Basically, it's the time it spends searching for the PlayerShip recursively. When the playership dies or is replaced, it's at the END, thus dramatically slowing this down.

