# Leonardo Luz Fachel - Software Analysis and Development Undergraduate Thesis

## Data Structure Game

**Keywords:** game, data structure, C#, CSharp, CS, MonoGame, education, code, programming, gamedev, tutorial, stack, queue, linked list, binary tree, sorting algorithms

## Description

This project aims to create an educational game that facilitates learning data structure concepts in a fun and engaging way.  Players will learn about various data structures through gameplay mechanics and progressively challenging levels.

## Technologies

* **Game Engine:** MonoGame (Consideration given to Unity and Godot)
* **Database:** SQLite

## Features

* **Parallax Backgrounds:**  To enhance visual appeal and immersion.

## Level Design

**Level 1: Basics Tutorial**

* **Inventory:** 1 slot.
* **Objective:**  Introduces basic movement and interaction.
* **Controls:**
    * A/Left Arrow: Move left
    * D/Right Arrow: Move right
    * S: Crouch
    * W/Space/Up Arrow: Jump
    * 1/2/3: Use item in the inventory slot.  (Only one slot available in this level)


**Level 2: Stack & Combos Tutorial**

* **Inventory:** Stack with a capacity of 5 items.
* **Objective:** Teaches stack mechanics (LIFO - Last-In, First-Out) and item combo effects.
* **Mechanics:** Combos are activated by using consecutively the same type of items in the inventory stack. The combo is broken if the stack's order is changed (e.g., by adding or removing an item).


**Level 3: Sort Consumables Tutorial**

* **Objective:** Introduces sorting algorithms through the use of consumables.
* **Mechanics:** Players use consumables (e.g., 'c' + inventory number) to sort elements based on their properties.  A visual representation of the sorting process will be displayed when consumables are used to showcase the chosen sorting algorithm (e.g., bubble sort or similar).
* **Combo Effects:**
    * Fire + Fire: 3 Fire Damage
    * Water + Water: 3 Water Damage
    * Wind + Wind: 3 Wind Damage
    * Earth + Earth: 3 Earth Damage
    * other combination in progress...


**Level 4: Queue & Multiple Inventory Tutorial**

* **Objective:** Introduces queue data structures (FIFO - First-In, First-Out) and managing multiple inventories.


**Level 5: Boss Fight (Stack/Queue Based)**

* **Objective:** A boss fight utilizing mechanics based on previously introduced stack and queue concepts.


**Level 6: Search Consumables Tutorial**

* **Objective:** Introduces searching algorithms through inventory management and finding specific consumables


**Level 7: Linked List Tutorial**

* **Objective:**  Introduces the concept of linked lists through gameplay mechanics.


**Level 8: Boss Fight (Linked List Based)**

* **Objective:** A boss fight utilizing mechanics based on linked list concepts.


**Post-Game Levels:**

* **Level 9: Double Linked List Tutorial**
* **Level 10: Binary Tree Tutorial**
* **Arena Mode:** An endless mode challenging the player with increasingly difficult scenarios incorporating all learned data structures.

## Art Style

* WIP

## Prototyping

* WIP

## Future Considerations

* Add more complex data structures (graphs, trees, hash tables)
* Implement difficulty levels
* Integrate a scoring system and leaderboards
* Add more diverse game mechanics and challenges


This README will be updated regularly to reflect the project's progress.
