# Cauldron Chaos

This is a small VR game made for the Virtual Reality course at the University of Applied Sciences Upper Austria by 

- [Christoph Daxerer](https://github.com/DrChristophFH)
- [Christopher Eberl]()

CC by CD and CE 2023 aka CxCDE 23 lul

## Description

Two or more players embark on a magical yet chaotic journey to master the strange art of potion crafting. One player takes the role of the apprentice, mixing and juggling the ingredients, while the other player takes the role of the master, who has the book of recipes and has to guide the apprentice through the process of creating the potion.

## Behind the Magic

This section describes the technical details of the game and how ingredients and recipes are handled.

### Property System

Ingredients are primarily characterized by their properties. They describe the compositional information of the ingredient in units. 

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
      - Paper
    - Bone
    - Flesh
    - Leather
  - Natural
    - Stone
    - Dirt
    - Sand
    - Clay
    - Glass
    - Gemstones
    - Crystal
  - Plastic
- Gas
- Magic Power
  - Water Magic
  - Fire Magic
  - Earth Magic
  - Wind Magic

As well as a characteristic.

- Characteristic
  - Explosive
  - Flammable
  - Poisonous
  - Radioactive
  - Magical
  - Edible
  - Normal

##### Creating materials

To define a material, `Create -> Cauldron Chaos -> Ingredient Material`. Place the material in the `Data` folder. You can select the parent of the material in the inspector. Children will be automatically added to the parent's properties.

### Ingredients

Ingredients are defined by a set of properties and a name. 

- **Apple**
  - Category: Fruit
  - Characteristic: Red, Edible
  - Material
    - Organic 10

#### Creating ingredients

To declare an object (or prefab) as an ingredient, add the `Ingredient` component to it. Define the name of the ingredient and add `Part`s to it. `Part`s are the composition of the ingredient. For example, an apple could be composed of 10 units of organic material.

### Recipes

Recipes are based upon a graph like structure. 

The cauldron always has a state in which it is in. The state is the primary source of visual and audio feedback.

Along with the state, the cauldron has a set of properties. These properties enable the cauldron to change its state. Those properties are:

- **Temperature**: The temperature of the cauldron in °C.
- **Volume**: The volume of the cauldron in ml.
- **Ingredients**: A list of all ingredients currently in the cauldron.
- **Duration**: The time the cauldron has been cooking in seconds.
- **Material**: Material left in the cauldron currently.

Each state has a set of transitions that will be taken once the cauldron has the required properties. An example could be:

- Example Transition
  - **From**: Water Base
  - **To**: Strange Liquid
  - **Requires**: 
    - Temperature > 50°C
    - Material
      - Organic 10 (consumed)
      - Water 5 (consumed)
    - Ingredient
      - 1x of "Magical" characteristic


## TODO

--- Technical ---
- [ ] Menu scene          CE
  - [ ] Start Button      CE
- [x] Create Cauldron State System
  - [ ] Visual Feedback: Entry, During (Exit uniform?) CD
  - [ ] Audio Feedback: Entry, During    CD
  - [ ] Cauldron Effects: Entry, During (Physics forces, spawn stuff etc.) CD
- [ ] Add sound effects (music, ingredient into cauldron) CD
- [ ] Create small task system ??

--- Gameplay ---
- [ ] Populate Level with ingredients CD/CE
- [ ] Fully create all materials   CE
  - [ ] Hierarchy                  CE
- [ ] Configure Ingredient stats   CD/CE
  - [ ] Balance
- [ ] Build States and Transitions 
- [ ] Create Player 2 Book

--- Polish ---
- [ ] Add physics interactions to world objects 
  - [ ] doors (circular drive or joints)
  - [ ] drawers (linear drives)
  - [ ] potions from ceiling(joints?)