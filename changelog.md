# Change Log

### v 0.07.0 - 10/27/2026
  * Added:
    - Added Mortar Tower
      - Added AOE projectile that can ether explode on conact or only on the ground
    - Added quick reset function
    - Added Game Over Screen that appears when player health is no longer positive
  * Changed:
    - Changed Arc Projectile code to use time instead of speed
    - Changed Tower Blueprint to abstract only using one prefab
      - Deleted Archer Tower Icon prefab
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
