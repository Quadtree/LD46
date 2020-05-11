This is a post mortem for my LD46 entry, Space Station Express.

# The Idea
Usually when the theme is announced I jot down a whole list of ideas, and then choose the one I like the most. For whatever reason, this theme only really made me think of one idea: transporting some live cargo in a spaceship. It seems like there are more ideas you could come up with from this theme, but that was the one I thought of. I decided to name it Space Station Express in a sort of vague Futurama reference.

# What Went Well
* I was using a new game engine for this one, Godot. Previously I've usually used UE4 for 3D stuff and libGDX for 2D stuff. I made a nearly complete game before the LD started to get some practice with Godot, and it mostly paid off in that I didn't have to deal with too many surprises (though, see below 🤦‍♂️).
* I'm pretty happy with the idea. I think it mostly hit the sweet spot of simple enough for new players but complex enough to allow for some varied gameplay.
* Most of the graphics. In particular, I'm pretty happy with how the background turned out.
* I used my [library](https://github.com/Quadtree/godotcslib) of helpful Godot C# functions, which came in handy. I even added a few new ones that I created during the LD.

# What Did Not Go Well
* For this project, I decided to go with Godot's most experimental flow, HTML5+Mono. This was only added in 3.2 and it's still very new and experimental. I'd used it before, so I thought I knew all the pitfalls, but as it turned out I did not. In particular, it seems that garbage collection doesn't work at all, which means that unless you can somehow have zero allocations, you will inevitably overflow the available memory leading to a crash. I tried various fixes, and at one point I thought the issue only affected Chrome, but as it turned out, it affects all browsers. I've opened a [bug](https://github.com/godotengine/godot/issues/38241) with the developers, but it looks like it's not going to be easy to fix, unfortunately.
* Sound levelling was another problem. For some reason, all sounds were shifted too high but I failed to notice this during testing. To help not blow out people's ears, I've reduced the sound level by 25 decibels.
* Some people mentioned that they couldn't figure out how much health they had. This confused me until I realized that I have a 1600 pixel tall monitor, and I'd foolishly set the window height to 1080. With the window decorations, this meant that for someone with a 1080p screen some parts of the UI would likely be pushed off the screen. This was why people were saying they couldn't figure out how much health they had. I fixed this by making the game fullscreen by default.
* It's fairly minor, but once again I forgot to allow for people with AZERTY keyboards. Nobody actually mentioned it this time, but I still should have remembered.
* Not directly related to the game, but it looks like I started judging other people's games too late. I'm only at 15 ratings even though I've rated 57 other games.

Anyway, if you want to try Space Station Express go [here](https://ldjam.com/events/ludum-dare/46/space-station-express). I'd really appreciate a few more ratings.
