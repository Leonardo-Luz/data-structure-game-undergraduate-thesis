# Leonardo Luz Fachel - Software Analysis and Development Undergraduate Thesis

## Data Structure Game

**Keywords:** game, data structure, C#, CSharp, CS, MonoGame, education, code, programming, gamedev, tutorial, stack, queue, linked list, binary tree, sorting algorithms

## Description

This project aims to create an educational game that facilitates learning data structure concepts in a fun and engaging way.  Players will learn about various data structures through gameplay mechanics and progressively challenging levels that incorporate alchemical themes.

## Technologies

* **Game Engine:** MonoGame (Consideration given to Unity and Godot)
* **Database:** SQLite

## Features

* **Parallax Backgrounds:**  To enhance visual appeal and immersion.

## History

The character is a alchemist that lost his bags and all he could found was a damaged bag,
he can produce some elements from time to time and can use them to defend himself.
his objective is to recover the bags and finish his research.

**Start**
alchemist fainted in the ground in the middle of a forest, a wagon wreckage by his right.
the alchemist wakes up.

**FIRST DIALOGUE**: What happened here... i can't remember almost anything... wait... MY BAGS. WHERE ARE MY MAGICAL BAGS.
**DIALOGUE: Pass through wagon wreckage:** maybe my bags can still be inside that wagon.

**DIALOGUE: interact with wagon:** my bags seem to be missing, probaly the same thing that did this to the wagon stole them... they left behind a damaged bag.

**DIALOGUE: Found damaged magical bag:** this can be usefull for i while. maybe i can store a element here, but only one... i still need the rest of my bags to complete my research, some monster might have stolen them.
**POPUP: element fabrication:** each x seconds the alchemist will make a new element, if the bag is full the production will stop.

**DIALOGUE: Found first enemy: (red slime, weakness: fire)** maybe i could use some of my elements to defeat it.
**POPUP: combat mechanics:** each enemy has it owns weaknesses and health points...

**MESSAGE: Uses Wrong element:** it seems to be weak to fire.


**found the stack bag dialogue**: Now that i have a bigger bag i can mix some elements. pop combination tutorial.

## Level Design

**Level 1: Basics Tutorial**

* **Inventory:** 1 slot.
* **Objective:** Introduces basic movement, interaction, cast time, and damage mechanics.

* **Controls:**
    * A/Left Arrow: Move left
    * D/Right Arrow: Move right
    * S: Crouch
    * W/Space/Up Arrow: Jump
    * 1/2/3: Use item in the inventory slot.  (Only one slot available in this level)

* **Mechanics:**: 
    * After using an item, there will be a timer that, when it reaches 0, will trigger the item’s use.
    * Enemies take damage only from their elemental weakness.

**Level 2: Stack & Combos Tutorial**

* **Inventory:** Stack with a capacity of 5 items.
* **Objective:** Teaches stack mechanics (LIFO - Last-In, First-Out) and item combo effects.
* **Mechanics:** Combos are activated by using items consecutively in the inventory. If the combo is invalid (e.g., fire + water + water), it gets disrupted, and the player takes damage.
* **See all combinations in [Combinations](#Combinations)**

**Level 3: Sort Consumables Tutorial**

* **Objective:** Introduces sorting algorithms through the use of consumables.
* **Mechanics:** Players use consumables (e.g., 'c' + inventory number) to sort elements based on their properties.  A visual representation of the sorting process will be displayed when consumables are used to showcase the chosen sorting algorithm (e.g., bubble sort or similar).

**Level 4: Queue & Multiple Inventory Tutorial**

* **Objective:** Introduces queue data structures (FIFO - First-In, First-Out) and managing multiple inventories.


**Level 5: Boss Fight (Stack/Queue Based)**

* **Objective:** A boss fight utilizing mechanics based on previously introduced stack and queue concepts.


**Level 6: Search Consumables Tutorial**

* **Objective:** Introduces searching algorithms through consumables


**Level 7: Linked List Tutorial**

* **Objective:**  Introduces the concept of linked lists through gameplay mechanics.


**Level 8: Boss Fight (Linked List Based)**

* **Objective:** A boss fight utilizing mechanics based on linked list concepts.


**Post-Game Levels:**

* **Level 9: Double Linked List Tutorial**
* **Level 10: Binary Tree Tutorial**
* **Arena Mode:** An endless mode challenging the player with increasingly difficult scenarios incorporating all learned data structures.

## Combinations

* Fire

| Element 1 | Element 2 | Result      |
|-----------|-----------|-------------|
| Fire      | Fire      | Ember       |
| Fire      | Water     | Steam       |
| Fire      | Wind      | Lightning   |
| Fire      | Earth     | Calcination |

* Water

| Element 1 | Element 2 | Result      |
|-----------|-----------|-------------|
| Water     | Fire      | Steam       |
| Water     | Water     | Wave        |
| Water     | Wind      |             |
| Water     | Earth     | Dissolution |

* Wind

| Element 1 | Element 2 | Result     |
|-----------|-----------|------------|
| Wind      | Fire      | Lightning  |
| Wind      | Water     |            |
| Wind      | Wind      |            |
| Wind      | Earth     | Separation |

* Earth

| Element 1 | Element 2 | Result      |
|-----------|-----------|-------------|
| Earth     | Fire      | Calcination |
| Earth     | Water     | Dissolution |
| Earth     | Wind      | Separation  |
| Earth     | Earth     | Conjuction  |

* Fire + Earth (Calcination)

| Element 1   | Element 2 | Result |
|-------------|-----------|--------|
| Calcination | Fire      | Copper |
| Calcination | Water     | Iron   |
| Calcination | Wind      | Silver |
| Calcination | Earth     | Gold   |

* Water + Earth (Dissolution)

| Element 1   | Element 2 | Result |
|-------------|-----------|--------|
| Dissolution | Fire      |        |
| Dissolution | Water     |        |
| Dissolution | Wind      |        |
| Dissolution | Earth     |        |

* Wind + Earth (Separation)

| Element 1  | Element 2 | Result |
|------------|-----------|--------|
| Separation | Fire      |        |
| Separation | Water     |        |
| Separation | Wind      |        |
| Separation | Earth     |        |

* Earth + Earth (Conjuction)

| Element 1  | Element 2 | Result |
|------------|-----------|--------|
| Conjuction | Fire      |        |
| Conjuction | Water     |        |
| Conjuction | Wind      |        |
| Conjuction | Earth     |        |

## Concept Arts

* These arts can be changed

## Prototyping

* WIP

## Future Considerations

* Add more complex data structures (graphs, trees, hash tables)
* Implement difficulty choice. ( Easy, Medium, Hard )
* Integrate a scoring system and leaderboards
* Add more diverse game mechanics and challenges

This README will be updated regularly to reflect the project's progress.
