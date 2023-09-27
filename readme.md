# Cauldron Chaos

This is a small VR game made for the Virtual Reality course at the University of Applied Sciences Upper Austria by 

- [Christoph Daxerer]()
- [Christopher Eberl]()

## Description

Two or more players embark on a magical yet chaotic journey to master the strange art of potion crafting. One player takes the role of the apprentice, mixing and juggling the ingredients, while the other player takes the role of the master, who has the book of recipes and has to guide the apprentice through the process of creating the potion.

## Behind the Magic

This section describes the technical details of the game and how ingredients and recipes are handled.

### Property System

Ingredients are primarily characterized by their properties. A set of values associated to them that describe their physical and magical properties. 

There are three types of properties:
- **Category**: A rough categorization of the ingredient.
- **Characteristic**: Describe the characteristics, like color etc.
- **Material**: Describe the compositional information of the ingredient in units.

#### Category Hierarchy

- Category (for rough categorization)
  - Nature 
    - Plant
      - Vegetable
      - Fruit
      - Mushroom
      - Flower
    - Animal
  - Man-made
    - Office Supplies
    - Weapon
      - Sword
      - Gun
      - Ammo
    - Game
      - Chess

#### Characteristic Hierarchy

- Characteristic
  - Color
    - Red
    - Green
    - Blue
    - Yellow
    - Orange
    - Purple
    - Pink
    - Black
    - White
    - Brown
    - Grey
  - Explosive
  - Flammable
  - Poisonous
  - Radioactive
  - Magical
  - Edible

#### Material Hierarchy

- Material (for rough compositional information)
  - Liquid
    - Water
  - Solid
    - Metal
      - Iron
      - Gold
      - Silver
      - Copper
      - Fancy (Base for all other fancy metals if we want to add more)
    - Organic
      - Wood
      - Bone
      - Flesh
      - Leather
    - Stone
    - Plastic
  - Gas

### Ingredients

Ingredients are defined by a set of properties and a name. 

- **Apple**
  - Category: Fruit
  - Characteristic: Red, Edible
  - Material
    - Organic 10

###