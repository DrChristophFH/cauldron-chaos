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

- [x] Create Cauldron State System
  - [x] Visual Feedback: Entry, During (Exit uniform?) 
  - [x] Cauldron Effects: Entry, During (Physics forces, spawn stuff etc.) 
- [x] Add sound effects (music, ingredient into cauldron) 

--- Gameplay ---

- [x] Populate Level with ingredients 
- [x] Fully create all materials 
  - [x] Hierarchy 
- [x] Configure Ingredient stats 
- [x] Build States and Transitions

--- Polish ---

- [x] Add physics interactions to world objects
  - [x] doors (circular drive or joints)
  - [x] drawers (linear drives)
  - [x] potions from ceiling(joints?)

--- Additions ---

- [x] Ingredient Inspect Station

--- Needs Fix ---

- [ ] Inspect station registers ingredient on pick up again
- [ ] reset ingredient (rare)
- [ ] show material graph table for VR player
- [ ] select quests based on difficulty
- [ ] more transitions