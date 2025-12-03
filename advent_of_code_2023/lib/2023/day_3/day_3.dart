import 'dart:io';
import 'dart:math';

import 'package:collection/collection.dart';

class Day3 {
  final numberExp = RegExp(r'\d');

  final numberCoordinates = <({int x, int y, int number})>[];
  final symbolCoordinates = <({int x, int y})>[];
  final gearCoordinates = <({int x, int y})>[];

  Future<int> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_3/day_3_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    _getCoordinatesFromLines(lines);

    return isPart2 ? _part2Answer() : _part1Answer();
  }

  void _getCoordinatesFromLines(List<String> lines) {
    numberCoordinates.clear();
    symbolCoordinates.clear();
    gearCoordinates.clear();

    // (0, 0) is top left corner
    for (var y = 0; y < lines.length; y++) {
      var buffer = StringBuffer();
      var line = ".${lines[y]}."; // Pad to avoid EOL
      var characters = line.split('');
      for (var x = 0; x < characters.length; x++) {
        var character = characters[x];
        var isNumber = numberExp.hasMatch(character);
        var isSymbol = character != '.' && !isNumber;
        var isGear = character == '*';

        if (isNumber) {
          buffer.write(character);
        } else if (isSymbol) {
          symbolCoordinates.add((x: x, y: y));
          if (isGear) {
            gearCoordinates.add((x: x, y: y));
          }
          _storeNumberFromBuffer(buffer, x, y);
        } else {
          _storeNumberFromBuffer(buffer, x, y);
        }
      }
      _storeNumberFromBuffer(buffer, characters.length, y);
    }
  }

  void _storeNumberFromBuffer(
    StringBuffer buffer,
    int x,
    int y,
  ) {
    if (buffer.isEmpty) return;

    var contents = buffer.toString();
    var number = int.parse(contents);

    numberCoordinates.add((number: number, x: x - contents.length, y: y));

    buffer.clear();
  }

  int _chebyshevDist(int x1, int x2, int y1, int y2) {
    return max((x1 - x2).abs(), (y1 - y2).abs());
  }

  int _part1Answer() {
    var partNumbers = <int>[];

    for (var part in numberCoordinates) {
      var length = (log(part.number) / log(10)).ceil();
      var y = part.y;
      for (var x = part.x; x < part.x + length; x++) {
        if (symbolCoordinates.any((coord) => _chebyshevDist(coord.x, x, coord.y, y) <= 1)) {
          partNumbers.add(part.number);
          break;
        }
      }
    }

    return partNumbers.sum;
  }

  int _part2Answer() {
    var possibleGears = <({int x, int y}), List<int>>{};
    var gearRatioTotal = 0;

    for (var gear in gearCoordinates) {
      for (var part in numberCoordinates) {
        var length = (log(part.number) / log(10)).ceil();
        var y = part.y;
        for (var x = part.x; x < part.x + length; x++) {
          if (_chebyshevDist(x, gear.x, y, gear.y) <= 1) {
            if (possibleGears[(x: gear.x, y: gear.y)] == null) {
              possibleGears[(x: gear.x, y: gear.y)] = [];
            }
            possibleGears[(x: gear.x, y: gear.y)]!.add(part.number);
            break;
          }
        }
      }
    }

    for (var possibleGear in possibleGears.values) {
      if (possibleGear.length == 2) {
        gearRatioTotal += possibleGear[0] * possibleGear[1];
      }
    }

    return gearRatioTotal;
  }
}
