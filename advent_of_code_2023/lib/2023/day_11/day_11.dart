import 'dart:io';
import 'dart:math';

import 'package:collection/collection.dart';

class Day11 {
  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_11/day_11_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var galaxies = <_Galaxy>[];
    var xMax = lines[0].length;
    var yMax = lines.length;

    var expansionAmount = (isPart2 ? 100000 : 2) - 1;

    lines.forEachIndexed((y, line) {
      line.codeUnits.forEachIndexed((x, codeUnit) {
        if (String.fromCharCode(codeUnit) == '#') {
          galaxies.add(_Galaxy(x: x, y: y));
        }
      });
    });

    for (var x = 0; x < xMax; x++) {
      if (!galaxies.any((galaxy) => galaxy.x == x)) {
        for (var galaxy in galaxies) {
          if (galaxy.x > x) galaxy.x += expansionAmount;
        }
        xMax += expansionAmount;
        x += expansionAmount;
      }
    }

    for (var y = 0; y < yMax; y++) {
      if (!galaxies.any((galaxy) => galaxy.y == y)) {
        for (var galaxy in galaxies) {
          if (galaxy.y > y) galaxy.y += expansionAmount;
        }
        yMax += expansionAmount;
        y += expansionAmount;
      }
    }

    var distanceTotal = 0;
    for (var galaxyA in galaxies) {
      for (var galaxyB in galaxies) {
        distanceTotal += _manhattanDistance(galaxyA, galaxyB);
      }
    }

    // Removes double counts
    return distanceTotal / 2;
  }

  int _manhattanDistance(_Galaxy a, _Galaxy b) {
    return (a.x - b.x).abs() + (a.y - b.y).abs();
  }
}

class _Galaxy {
  int x;
  int y;

  _Galaxy({required this.x, required this.y});

  @override
  String toString() {
    return "($x - $y)";
  }
}
