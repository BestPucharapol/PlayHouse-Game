# PlayHouse - Game

The PlayHouse project contains 2 parts: the Arduino board, and the Unity game. This is the repository of the game part of the project. For the Arduino board part, please check out this [repository]().

The project is part of 261492 Project of Computer Engineering Bachelor Degree.

## Installation

1. Download and install both [Unity Hub](https://unity.com/download) and [Unity LTS 2021.3.35](https://unity.com/releases/editor/archive) or newer. Note that if you do not download the latest version of Unity, you will need to locate the editor in Unity Hub.
2. Download and extract this repository.
3. Open the project with Unity Hub.

## Usage

In order to properly configure the values, please make sure that you can connect to the Arduino board with Arduino IDE and are able to use the `Serial Monitor` of the IDE. The values used here will be the same as in the IDE.

1. Open the scene `PlayHouse` in `Scenes` folder.
2. Inspect the `SerialController` GameObject in `Hierarchy` and configure the values under `Serial Controller (Script)`.
3. Connect the Arduino board the your PC.

## Troubleshooting

- If Unity fails to connect to Arduino board, please check that there is no other process accessing the serial communication port of Arduino, e.g., the `Serial Monitor` of Arduino IDE.
