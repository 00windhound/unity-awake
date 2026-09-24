this simulation allows you to watch evolution take place.
plants are able to change color and shape with each new generation.

Tab: change controls
E: pick up
Shift: bulldoze

Optimizations

A world will be expected to have thousands of living plants and animals in it. it is important to plan for optimized performance early and to build and design with that in mind. initially i had an update function on each plant that they used to trigger their own growth and behaviors, but with potentially thousands of plants each with an active update function, i found that lagg and performance issues where quickly becoming a struggle.

my solution was to have the global script keep a running list of all the plants in the game and loop through each plant at a controlled pace and trigger their update functions. plants no longer have a true update function. they are only active while the global script directly calls their new update function.

