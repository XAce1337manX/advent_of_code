import 'dart:io';

class Day12 {
  final dotCodeUnit = '.'.codeUnits.first;
  final questionCodeUnit = '?'.codeUnits.first;
  final hashCodeUnit = '#'.codeUnits.first;
  final memo = <(int arrangementIndex, int brokenStreak, int springIndex), int>{};

  String arrangement = "";
  List<int> brokenSprings = [];

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_12/day_12_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var total = 0;
    for (var line in lines) {
      var split = line.split(' ');
      arrangement = split[0];
      brokenSprings = split[1].split(',').map((e) => int.parse(e)).toList();

      if (!isPart2) {
        arrangement += '.';
      } else {
        arrangement = "$arrangement?$arrangement?$arrangement?$arrangement?$arrangement.";
        brokenSprings = [
          ...brokenSprings,
          ...brokenSprings,
          ...brokenSprings,
          ...brokenSprings,
          ...brokenSprings
        ];
      }

      memo.clear();
      var result = _recur(0, 0, 0);
      total += result;
    }
//

    return total;
  }

  int _recur(
    int arrangementIndex,
    int brokenStreak,
    int springIndex,
  ) {
    var key = (arrangementIndex, brokenStreak, springIndex);
    if (memo.containsKey(key)) {
      return memo[key]!;
    }

    // Base case - End of string
    if (arrangementIndex == arrangement.length) {
      var isValidArrangement = springIndex == brokenSprings.length && brokenStreak == 0;
      var result = isValidArrangement ? 1 : 0;
      memo[key] = result;
      return result;
    }

    var char = arrangement.codeUnitAt(arrangementIndex);

    if (char == dotCodeUnit) {
      // Mismatched spring group
      if (brokenStreak > 0 && brokenStreak != brokenSprings[springIndex]) {
        return _getFromMemo(key, () => 0);
      }

      // Previous was functional
      if (brokenStreak == 0) {
        return _getFromMemo(key, () => _recur(arrangementIndex + 1, 0, springIndex));
      }

      // Close off broken spring group
      return _getFromMemo(key, () => _recur(arrangementIndex + 1, 0, springIndex + 1));
    }

    if (char == questionCodeUnit) {
      // Previous was functional
      if (brokenStreak == 0) {
        // Enough groups - must be functional
        if (springIndex == brokenSprings.length) {
          return _getFromMemo(key, () => _recur(arrangementIndex + 1, 0, springIndex));
        }

        // Not enough groups - could be broken or functional
        return _getFromMemo(
            key,
            () =>
                _recur(arrangementIndex + 1, 0, springIndex) +
                _recur(arrangementIndex + 1, 1, springIndex));
      }

      // Previous was broken
      if (brokenStreak > 0) {
        // Too many groups
        if (springIndex == brokenSprings.length) {
          return _getFromMemo(key, () => 0);
        }

        // Must close group
        if (brokenStreak == brokenSprings[springIndex]) {
          return _getFromMemo(key, () => _recur(arrangementIndex + 1, 0, springIndex + 1));
        }

        // Must continue group
        return _getFromMemo(key, () => _recur(arrangementIndex + 1, brokenStreak + 1, springIndex));
      }
    }

    if (char == hashCodeUnit) {
      // Too many spring groups
      if (springIndex == brokenSprings.length) {
        return _getFromMemo(key, () => 0);
      }

      // Spring group too big
      if (brokenStreak >= brokenSprings[springIndex]) {
        return _getFromMemo(key, () => 0);
      }

      // Continue group
      return _getFromMemo(key, () => _recur(arrangementIndex + 1, brokenStreak + 1, springIndex));
    }

    return 9999999999999999;
  }

  int _getFromMemo((int, int, int) key, int Function() value) {
    if (memo.containsKey(key)) {
      return memo[key]!;
    }

    var result = value();
    memo[key] = result;
    return result;
  }
}
