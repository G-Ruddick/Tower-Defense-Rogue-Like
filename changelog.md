# Change Log

### v 0.10.0 - 11/17/2026
  * Added:
    - Added win game screen and win condition at level 20
    - Added sell value display to tower ui
  * Changed:
    - Buying tower upgrades now increase tower sell value
  * Bug Fix: 
    - Killing all currently spawned enemies no longer counts as completing a wave

### v 0.09.0 - 11/10/2026
  * Added:
    - Added new next wave button sprite
    - Added Gold Mine Tower
    - Created functionality for Generative Tower Class
      - Added generative types
      - Made generative tower only active durring active waves
  * Changed:
    - Clicking on multiple towers now hides previous towers' UI
    - Tower UI now blocks players mouse from selecting other gameobjects
    - Player can no longer go to next level until all enemies are killed
    - Exiting the shop no longer starts next wave
  * Bug Fix: 
    - AOE projectiles no longer deal damage before landing on the ground or target
    - Tower UI now is effected by changing the game brightness in settings

### v 0.08.0 - 11/3/2026
  * Added:
    - Added functionality to map display button in shop allowing player to view but not interact  with the game map
    - Added functionality to brightness slider allowing the player to make the game darker
    - Added Tower Upgrading Allowing for the player to spend money to upgrade and/or enhance towers
  * Bug Fix:
    - Entering settings now pauses the game properly

### v 0.07.0 - 10/27/2026
  * Added:
    - Added Mortar Tower
      - Added AOE projectile that can ether explode on conact or only on the ground
    - Added Mage Tower
      - Added Homing projectile that follows enemy until it hits target
    - Added quick reset function
    - Added Game Over Screen that appears when player health is no longer positive
  * Changed:
    - Changed Arc Projectile code to use time instead of speed
    - Changed Tower Blueprint to abstract only using one prefab
      - Deleted Archer Tower Icon prefab
    - Increased Archer Tower Range
  * Bug Fix: 
    - Shop icon now shows correct tower icon
    - Grid no longer appears on game load and reset

### v 0.06.0 - 10/20/2026
  * Added:
    - Added Change Log
    - Added walking animation to enemies
    - Added firing animations to Archer Tower
    - Added grid on screen to show available tower placing locations
  * Bug Fix:
    - Menu no longer shows quit button in game scene and instead shows return button which takes player back to title screen

### v 0.05.0 - 10/13/2026
  * Added:
    - Added wave difficulty scaling
    - Added Mage and Dark Knight enemies
    - Added enemy wave cost calculation

### v 0.04.0 - 10/6/2026
  * Added:
    - Added Projectile class that travels using physics engine and hurts enemies
      - Towers no longer damage enemies just for being in range
    - Added Arching Projectile class that has uses gravity
    - Added health bar to injured enemies to display health percentage
    - Added Spawning positions to enemies
    - Added wave difficulty scaling

### v 0.03.0 - 9/29/2026
  * Added:
    - Added Tower Blueprints available to be purchased from shop if tower is not owned by player
    - Added tower range sprites
    - Added tower damaging enemies within range
    - Added tower cursor display when placing towers

### v 0.02.0 - 9/22/2026
  * Added:
    - Added Map Generation and creation
    - Added Map Tiles
      - Added Path tiles, Grass tiles, and Water tiles
    - Added Base Tower, Attack Tower, Generative Tower and Troop Tower classes
    - Added Archer tower
      - Made tower placeable on grass tiles
      - Made towers sellable
    - Added Archer tower blueprint to buy towers
    - Added Goblin enemy
    - Added Enemy Spawning

### v 0.01.0 - 9/15/2026
  * Added:
    - Added title screen
    - Added Settings menu
    - Added Shop screen
    - Added Game scene
    - Added pause menu
    - added player stats
