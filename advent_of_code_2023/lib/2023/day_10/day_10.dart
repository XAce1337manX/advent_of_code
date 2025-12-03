import 'dart:io';

import 'package:collection/collection.dart';

class Day10 {
  final grid = <({int x, int y}), String>{};

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_10/day_10_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var xMax = lines[0].length;
    var yMax = lines.length;

    grid.clear();

    lines.forEachIndexed((y, line) {
      line.codeUnits.forEachIndexed((x, codeUnit) {
        grid[(x: x, y: y)] = String.fromCharCode(codeUnit);
      });
    });
    var start = grid.entries.where((element) => element.value == 'S').first;

    var loopCoordinates = [start.key];

    for (var startDirection in _Direction.values) {
      if (_canMove(start.key, startDirection, xMax, yMax)) {
        var loopLength = 0;
        var nextCoordinate = _nextCoordinate(start.key, startDirection);
        var nextDirection = _nextDirection(nextCoordinate, startDirection);
        while (nextCoordinate != start.key) {
          loopCoordinates.add(nextCoordinate);
          nextCoordinate = _nextCoordinate(nextCoordinate, nextDirection);
          nextDirection = _nextDirection(nextCoordinate, nextDirection);
          loopLength++;
        }
        if (!isPart2) {
          return (loopLength / 2).ceil();
        } else {
          // This section is for replacing S with the correct character
          if ((startDirection == _Direction.up && nextDirection == _Direction.up) ||
              (startDirection == _Direction.down && nextDirection == _Direction.down)) {
            grid[start.key] = '|';
          } else if ((startDirection == _Direction.left && nextDirection == _Direction.left) ||
              (startDirection == _Direction.right && nextDirection == _Direction.right)) {
            grid[start.key] = '-';
          } else if ((startDirection == _Direction.up && nextDirection == _Direction.right) ||
              (startDirection == _Direction.left && nextDirection == _Direction.down)) {
            grid[start.key] = 'J';
          } else if ((startDirection == _Direction.right && nextDirection == _Direction.up) ||
              (startDirection == _Direction.down && nextDirection == _Direction.left)) {
            grid[start.key] = 'F';
          } else if ((startDirection == _Direction.up && nextDirection == _Direction.left) ||
              (startDirection == _Direction.right && nextDirection == _Direction.down)) {
            grid[start.key] = 'L';
          } else if ((startDirection == _Direction.left && nextDirection == _Direction.up) ||
              (startDirection == _Direction.down && nextDirection == _Direction.right)) {
            grid[start.key] = '7';
          }

          // And now for area featuring parity
          var enclosedTiles = 0;
          for (var y = 0; y < yMax; y++) {
            var corners = <String>[];
            var isInsideLoop = false;
            for (var x = 0; x < xMax; x++) {
              var tile = grid[(x: x, y: y)];

              if (loopCoordinates.contains((x: x, y: y))) {
                if (tile == 'J' || tile == 'F' || tile == 'L' || tile == '7') {
                  corners.add(tile!);
                  if (corners.length == 2) {
                    if ((corners.contains('F') && corners.contains('J')) ||
                        (corners.contains('L') && corners.contains('7'))) {
                      isInsideLoop = !isInsideLoop;
                    }
                    corners.clear();
                  }
                } else if (tile == '|') {
                  isInsideLoop = !isInsideLoop;
                }
              } else if (isInsideLoop) {
                enclosedTiles++;
              }
            }
          }
          return enclosedTiles;
        }
      }
    }

    return -1;
  }

  // Assume movement was legal
  _Direction _nextDirection(({int x, int y}) coordinate, _Direction prevDirection) {
    var tile = grid[coordinate];

    if (prevDirection == _Direction.up) {
      return switch (tile) {
        '|' => _Direction.up,
        '7' => _Direction.left,
        'F' => _Direction.right,
        _ => prevDirection,
      };
    } else if (prevDirection == _Direction.down) {
      return switch (tile) {
        '|' => _Direction.down,
        'L' => _Direction.right,
        'J' => _Direction.left,
        _ => prevDirection,
      };
    } else if (prevDirection == _Direction.left) {
      return switch (tile) {
        '-' => _Direction.left,
        'L' => _Direction.up,
        'F' => _Direction.down,
        _ => prevDirection,
      };
    } else {
      return switch (tile) {
        '-' => _Direction.right,
        '7' => _Direction.down,
        'J' => _Direction.up,
        _ => prevDirection,
      };
    }
  }

  ({int x, int y}) _nextCoordinate(({int x, int y}) coordinate, _Direction direction) {
    return switch (direction) {
      _Direction.down => ((x: coordinate.x, y: coordinate.y + 1)),
      _Direction.up => ((x: coordinate.x, y: coordinate.y - 1)),
      _Direction.left => ((x: coordinate.x - 1, y: coordinate.y)),
      _Direction.right => ((x: coordinate.x + 1, y: coordinate.y)),
    };
  }

  bool _canMove(({int x, int y}) coordinate, _Direction direction, int xMax, int yMax) {
    var checkCoordinate = _nextCoordinate(coordinate, direction);
    if (checkCoordinate.x < 0 ||
        checkCoordinate.y < 0 ||
        checkCoordinate.x >= xMax ||
        checkCoordinate.y >= yMax) {
      return false;
    }

    var newTile = grid[checkCoordinate];
    return switch (newTile) {
      '|' => direction == _Direction.down || direction == _Direction.up,
      '-' => direction == _Direction.left || direction == _Direction.right,
      'L' => direction == _Direction.left || direction == _Direction.down,
      'F' => direction == _Direction.left || direction == _Direction.up,
      'J' => direction == _Direction.down || direction == _Direction.right,
      '7' => direction == _Direction.right || direction == _Direction.up,
      _ => false,
    };
  }
}

enum _Direction { up, down, left, right }
