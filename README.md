# PlayHouse - Game

The PlayHouse project contains 2 parts: the Arduino board, and the Unity game. This is the repository of the game part of the project. For the Arduino board part, check out this [repository](https://github.com/Som3on31/PlayHouse-IoT).

The project is part of 261492 Project of Computer Engineering Bachelor Degree.

## Installation

1. Download and install both [Unity Hub](https://unity.com/download) and [Unity LTS 2021.3.35](https://unity.com/releases/editor/archive) or newer. Note that if you do not download the latest version of Unity, you will need to locate the editor in Unity Hub.
2. Download and extract this repository.
3. Open the project with Unity Hub.

## Usage

In order to properly configure the values, make sure that you can connect to the Arduino board with Arduino IDE and are able to use the `Serial Monitor` of the IDE. The values used here will be the same as in the IDE.

1. Open the scene `PlayHouse` in `Scenes` folder.
2. Inspect the `SerialController` GameObject in `Hierarchy` and configure the values under `Serial Controller (Script)`.
3. Connect the Arduino board the your PC.

## About Arduino boards

There are [2 types of Arduino board](https://youtu.be/874COriDXcM?si=JfXqSHITAwyQpiT1): genuine and clone. Genuine boards come directly from Arduino's own manufacturing and are more expensive. Clone boards are manufactured by third-party sources, are much cheaper, and are widely available. Both types have the same functionalities and are interchangeable with each other since the schematics are open-source.

Clone boards use [different USB to serial chip](https://cdn.sparkfun.com/datasheets/Dev/Arduino/Other/CH340DS1.PDF) which requires different driver to function. For more info on how to setup your Arduino board, check out these links:

- For clone board, see [How to Install CH340 Drivers](https://learn.sparkfun.com/tutorials/how-to-install-ch340-drivers/all)
- For genuine board, see [Getting Started with Arduino UNO](https://www.arduino.cc/en/Guide/ArduinoUno)

## Troubleshooting

- If Unity fails to connect to Arduino board, check that there is no other process accessing the serial communication port of Arduino, e.g., the `Serial Monitor` of Arduino IDE.
