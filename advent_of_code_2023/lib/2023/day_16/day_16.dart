import 'dart:io';
import 'dart:math';

class Day16 {
  final dotCodeUnit = '.'.codeUnitAt(0);
  final forwSlashCodeUnit = '/'.codeUnitAt(0);
  final backSlashCodeUnit = '\\'.codeUnitAt(0);
  final dashCodeUnit = '-'.codeUnitAt(0);
  final pipeCodeUnit = '|'.codeUnitAt(0);
  final alreadyTraversedTiles = <({int x, int y, _Direction direction})>[];

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_16/day_16_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var grid = <List<({int codeUnit, bool isEnergised})>>[];
    for (var line in lines) {
      grid.add(line.codeUnits
          .map((codeUnit) => ((
                codeUnit: codeUnit,
                isEnergised: false,
              )))
          .toList());
    }

    var totalEnergised = 0;
    if (!isPart2) {
      _resetGrid(grid);
      _energiseTile(grid, 0, 0, _Direction.right);
      return _totalEnergised(grid);
    }

    // lol this takes 5 minutes because I throw away a lot of info
    for (var x = 0; x < grid[0].length; x++) {
      _resetGrid(grid);
      _energiseTile(grid, 0, x, _Direction.down);
      totalEnergised = max(totalEnergised, _totalEnergised(grid));
    }
    for (var x = 0; x < grid[0].length; x++) {
      _resetGrid(grid);
      _energiseTile(grid, grid.length - 1, x, _Direction.up);
      totalEnergised = max(totalEnergised, _totalEnergised(grid));
    }
    for (var y = 0; y < grid.length; y++) {
      _resetGrid(grid);
      _energiseTile(grid, y, 0, _Direction.right);
      totalEnergised = max(totalEnergised, _totalEnergised(grid));
    }
    for (var y = 0; y < grid.length; y++) {
      _resetGrid(grid);
      _energiseTile(grid, y, grid[0].length - 1, _Direction.left);
      totalEnergised = max(totalEnergised, _totalEnergised(grid));
    }

    return totalEnergised;
  }

  int _totalEnergised(List<List<({int codeUnit, bool isEnergised})>> grid) {
    var totalEnergised = 0;
    for (var line in grid) {
      for (var tile in line) {
        if (tile.isEnergised) {
          totalEnergised += 1;
        }
      }
    }
    return totalEnergised;
  }

  void _resetGrid(List<List<({int codeUnit, bool isEnergised})>> grid) {
    for (var y = 0; y < grid.length; y++) {
      for (var x = 0; x < grid[0].length; x++) {
        grid[y][x] = (codeUnit: grid[y][x].codeUnit, isEnergised: false);
      }
    }
    alreadyTraversedTiles.clear();
  }

  void _energiseTile(
    List<List<({int codeUnit, bool isEnergised})>> grid,
    int y,
    int x,
    _Direction direction,
  ) {
    if (x < 0 || x >= grid[0].length || y < 0 || y >= grid.length) {
      return;
    }

    var tileDirection = (x: x, y: y, direction: direction);
    if (alreadyTraversedTiles.contains(tileDirection)) {
      return;
    } else {
      alreadyTraversedTiles.add(tileDirection);
    }

    grid[y][x] = ((codeUnit: grid[y][x].codeUnit, isEnergised: true));

    var codeUnit = grid[y][x].codeUnit;
    if (codeUnit == dotCodeUnit) {
      switch (direction) {
        case _Direction.up:
          _energiseTile(grid, y - 1, x, direction);
          break;
        case _Direction.down:
          _energiseTile(grid, y + 1, x, direction);
          break;
        case _Direction.left:
          _energiseTile(grid, y, x - 1, direction);
          break;
        case _Direction.right:
          _energiseTile(grid, y, x + 1, direction);
          break;
      }
    } else if (codeUnit == dashCodeUnit) {
      switch (direction) {
        case _Direction.up:
        case _Direction.down:
          _energiseTile(grid, y, x - 1, _Direction.left);
          _energiseTile(grid, y, x + 1, _Direction.right);
          break;
        case _Direction.left:
          _energiseTile(grid, y, x - 1, direction);
          break;
        case _Direction.right:
          _energiseTile(grid, y, x + 1, direction);
          break;
      }
    } else if (codeUnit == pipeCodeUnit) {
      switch (direction) {
        case _Direction.up:
          _energiseTile(grid, y - 1, x, direction);
          break;
        case _Direction.down:
          _energiseTile(grid, y + 1, x, direction);
          break;
        case _Direction.left:
        case _Direction.right:
          _energiseTile(grid, y - 1, x, _Direction.up);
          _energiseTile(grid, y + 1, x, _Direction.down);
          break;
      }
    } else if (codeUnit == forwSlashCodeUnit) {
      switch (direction) {
        case _Direction.up:
          _energiseTile(grid, y, x + 1, _Direction.right);
          break;
        case _Direction.down:
          _energiseTile(grid, y, x - 1, _Direction.left);
          break;
        case _Direction.left:
          _energiseTile(grid, y + 1, x, _Direction.down);
          break;
        case _Direction.right:
          _energiseTile(grid, y - 1, x, _Direction.up);
          break;
      }
    } else if (codeUnit == backSlashCodeUnit) {
      switch (direction) {
        case _Direction.up:
          _energiseTile(grid, y, x - 1, _Direction.left);
          break;
        case _Direction.down:
          _energiseTile(grid, y, x + 1, _Direction.right);
          break;
        case _Direction.left:
          _energiseTile(grid, y - 1, x, _Direction.up);
          break;
        case _Direction.right:
          _energiseTile(grid, y + 1, x, _Direction.down);
          break;
      }
    }
  }
}

enum _Direction { up, down, left, right }
