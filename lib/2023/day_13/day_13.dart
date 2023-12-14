import 'dart:io';

import 'package:collection/collection.dart';

// Haha brute force
class Day13 {
  final listComparer = const ListEquality();
  final hashCodeUnit = '#'.codeUnits.first;
  final dotCodeUnit = '.'.codeUnits.first;

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_13/day_13_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();
    lines.add("");

    var grids = <List<List<int>>>[];
    var grid = <List<int>>[];
    for (var line in lines) {
      if (line.isNotEmpty) {
        grid.add(line.codeUnits.toList());
      } else {
        grids.add(grid);
        grid = [];
      }
    }

    var total = 0;
    for (var grid in grids) {
      if (isPart2) {
        total += _smudgedValue(grid);
        continue;
      }

      var horizontalMirrors = _findMirrors(grid, transposeGrid: false);
      if (horizontalMirrors.isNotEmpty) {
        total += 100 * (horizontalMirrors.first + 1);
        continue;
      }

      var verticalMirrors = _findMirrors(grid, transposeGrid: true);
      if (verticalMirrors.isNotEmpty) {
        total += verticalMirrors.first + 1;
      }
    }

    return total;
  }

  // Finds horizontal mirrors - transpose for vertical mirror
  List<int> _findMirrors(List<List<int>> grid, {required bool transposeGrid}) {
    var mirrors = <int>[];

    if (transposeGrid) {
      var transposedGrid = <List<int>>[];
      for (var x = 0; x < grid[0].length; x++) {
        var row = <int>[];
        for (var y = 0; y < grid.length; y++) {
          row.add(grid[y][x]);
        }
        transposedGrid.add(row);
      }
      grid = transposedGrid;
    }

    var yMax = grid.length;
    for (var y = 0; y < yMax - 1; y++) {
      // Possible mirror. Check if it's actually a mirror
      if (listComparer.equals(grid[y], grid[y + 1])) {
        var beforeIndex = y - 1;
        var afterIndex = y + 1 + 1;
        var isMirror = true;
        while (isMirror && beforeIndex >= 0 && afterIndex < yMax) {
          if (!(listComparer.equals(grid[beforeIndex], grid[afterIndex]))) {
            isMirror = false;
          }
          beforeIndex--;
          afterIndex++;
        }

        if (isMirror) {
          mirrors.add(y);
        }
      }
    }

    return mirrors;
  }

  int _smudgedValue(List<List<int>> grid) {
    var yMax = grid.length;
    var xMax = grid[0].length;
    var totalValues = xMax * yMax;

    var originalHorizontalMirror = _findMirrors(grid, transposeGrid: false).firstOrNull;
    var originalVerticalMirror = _findMirrors(grid, transposeGrid: true).firstOrNull;

    for (var i = 0; i < totalValues; i++) {
      // Smudge it
      var originalCodeUnit = grid[i ~/ xMax][i % xMax];
      grid[i ~/ xMax][i % xMax] = (originalCodeUnit == hashCodeUnit) ? dotCodeUnit : hashCodeUnit;

      var horizontalMirrors = _findMirrors(grid, transposeGrid: false)
          .where((mirrorIndex) => mirrorIndex != originalHorizontalMirror);
      if (horizontalMirrors.isNotEmpty) {
        return 100 * (horizontalMirrors.first + 1);
      }

      var verticalMirrors = _findMirrors(grid, transposeGrid: true)
          .where((mirrorIndex) => mirrorIndex != originalVerticalMirror);
      if (verticalMirrors.isNotEmpty) {
        return verticalMirrors.first + 1;
      }

      // Unsmudge it
      grid[i ~/ xMax][i % xMax] = originalCodeUnit;
    }

    return -9999999999999;
  }
}
