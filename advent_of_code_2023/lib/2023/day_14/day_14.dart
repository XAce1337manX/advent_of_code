import 'dart:io';

import 'package:collection/collection.dart';

class Day14 {
  final listComparer = const DeepCollectionEquality();
  final oCodeUnit = 'O'.codeUnits.first;
  final hashCodeUnit = '#'.codeUnits.first;
  final dotCodeUnit = '.'.codeUnits.first;

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_14/day_14_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var grid = <List<int>>[];
    for (var line in lines) {
      grid.add(line.codeUnits.toList());
    }

    if (!isPart2) {
      _moveRocksUp(grid);
    } else {
      // Haha dish go speeeeeenn
      var isPeriodFound = false;
      var cycles = 0;
      var cycleLimit = 1000000000;
      var cycleStates = <List<List<int>>>[];
      cycleStates.add(_deepCopyGrid(grid));
      while (cycles < cycleLimit) {
        _moveRocksUp(grid);
        _moveRocksLeft(grid);
        _moveRocksDown(grid);
        _moveRocksRight(grid);
        cycles++;

        if (!isPeriodFound) {
          var periodIndex =
              cycleStates.indexWhere((cycleGrid) => listComparer.equals(cycleGrid, grid));
          if (periodIndex >= 0) {
            print("Found match between $periodIndex and $cycles - Period ${cycles - periodIndex}");
            isPeriodFound = true;
            var period = cycles - periodIndex;
            var cyclesToSkip = (cycleLimit - cycles) ~/ period * period;
            cycles += cyclesToSkip;
          }
          cycleStates.add(_deepCopyGrid(grid));
        }
      }
    }

    // Calculate
    var total = 0;
    for (var i = 0; i < grid.length; i++) {
      for (var tile in grid[i]) {
        total += (tile == oCodeUnit) ? grid.length - i : 0;
      }
    }

    return total;
  }

  List<List<int>> _deepCopyGrid(List<List<int>> grid) {
    return grid.map((number) => List<int>.from(number)).toList();
  }

  void _moveRocksUp(List<List<int>> grid) {
    for (var y = 0; y < grid.length; y++) {
      for (var x = 0; x < grid[y].length; x++) {
        if (grid[y][x] == dotCodeUnit) {
          for (var yCheck = y + 1; yCheck < grid.length; yCheck++) {
            var tile = grid[yCheck][x];
            if (tile == hashCodeUnit) {
              break;
            } else if (tile == oCodeUnit) {
              grid[y][x] = oCodeUnit;
              grid[yCheck][x] = dotCodeUnit;
              break;
            }
          }
        }
      }
    }
  }

  void _moveRocksDown(List<List<int>> grid) {
    for (var y = grid.length - 1; y >= 0; y--) {
      for (var x = grid[y].length - 1; x >= 0; x--) {
        if (grid[y][x] == dotCodeUnit) {
          for (var yCheck = y - 1; yCheck >= 0; yCheck--) {
            var tile = grid[yCheck][x];
            if (tile == hashCodeUnit) {
              break;
            } else if (tile == oCodeUnit) {
              grid[y][x] = oCodeUnit;
              grid[yCheck][x] = dotCodeUnit;
              break;
            }
          }
        }
      }
    }
  }

  void _moveRocksLeft(List<List<int>> grid) {
    for (var x = 0; x < grid[0].length; x++) {
      for (var y = 0; y < grid.length; y++) {
        if (grid[y][x] == dotCodeUnit) {
          for (var xCheck = x + 1; xCheck < grid.length; xCheck++) {
            var tile = grid[y][xCheck];
            if (tile == hashCodeUnit) {
              break;
            } else if (tile == oCodeUnit) {
              grid[y][x] = oCodeUnit;
              grid[y][xCheck] = dotCodeUnit;
              break;
            }
          }
        }
      }
    }
  }

  void _moveRocksRight(List<List<int>> grid) {
    for (var x = grid[0].length - 1; x >= 0; x--) {
      for (var y = grid.length - 1; y >= 0; y--) {
        if (grid[y][x] == dotCodeUnit) {
          for (var xCheck = x - 1; xCheck >= 0; xCheck--) {
            var tile = grid[y][xCheck];
            if (tile == hashCodeUnit) {
              break;
            } else if (tile == oCodeUnit) {
              grid[y][x] = oCodeUnit;
              grid[y][xCheck] = dotCodeUnit;
              break;
            }
          }
        }
      }
    }
  }
}
