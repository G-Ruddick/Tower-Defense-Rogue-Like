# Change Log

### v 0.07.0 - 10/27/2026
  * Added Mortar Tower
    - Added AOE projectile that can ether explode on conact or only on the ground
  * Changed Arc Projectile code to use time instead of speed
  * Bug Fix: Shop Icon not using correct icon positioning when being created
  * Changed Tower Blueprint to abstract only using one prefab
    - Deleted Archer Tower Icon prefab
  * 
  * 

### v 0.06.0 - 10/20/2026
  * Added walking animation to enemies
  * Bug fix menu pause screen quitting game instead of returning to title
  * Added grid on screen to show available tower placing locations
  * Added firing animations to Archer Tower

### v 0.05.0 - 10/13/2026
  * Added wave difficulty scaling
  * Added Mage and Dark Knight enemies
  * Added enemy wave cost calculation

### v 0.04.0 - 10/6/2026
  * Added Projectile class that travels using physics engine and hurts enemies
    - Towers no longer damage enemies just for being in range
  * Added Arching Projectile class that has uses gravity
  * Added health bar to injured enemies to display health percentage
  * Added Spawning positions to enemies
  * Added wave difficulty scaling

### v 0.03.0 - 9/29/2026
  * Made Tower Blueprints available to be purchased from shop if tower is not owned by player
  * Made towers show their range by selecting
  * Made towers damage enemies within range
  * Made towers display their location as a transparent deactivated version when buying and placing a tower
  * Refactored Tower Blueprint node to be general and used as a blueprint for any tower

### v 0.02.0 - 9/22/2026
  * Added Map Generation and creation
  * Added Map Tiles
    - Added Path tiles, Grass tiles, and Water tiles
  * Added Base Tower, Attack Tower, Generative Tower and Troop Tower classes
  * Added Archer tower
    - Made tower placeable on grass tiles
    - Made towers sellable
  * Added Archer tower blueprint to buy towers
  * Added Goblin enemy
  * Added Enemy Spawning

### v 0.01.0 - 9/15/2026
  * Added title screen
  * Added Settings menu
  * Added Shop screen
  * Added Game scene
  * Added pause menu
  * added player stats
