import 'dart:io';

class Day8 {
  final letterExp = RegExp(r'[A-Za-z]+');
  final codeUnitL = 'L'.codeUnits.first;
  final codeUnitA = 'A'.codeUnits.first;
  final codeUnitZ = 'Z'.codeUnits.first;

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_8/day_8_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var directions = lines[0];

    var startAs = <String>[];
    var endZs = <String>[];

    var network = <String, ({String left, String right})>{};
    for (var i = 2; i < lines.length; i++) {
      var labels = letterExp.allMatches(lines[i]).toList();
      var start = labels[0].group(0)!;
      var left = labels[1].group(0)!;
      var right = labels[2].group(0)!;
      network[start] = (left: left, right: right);

      if (start.codeUnits.last == codeUnitA) {
        startAs.add(start);
      } else if (start.codeUnits.last == codeUnitZ) {
        endZs.add(start);
      }
    }

    if (isPart2) {
      var stepsRequired = <int>[];
      for (var start in startAs) {
        var stepTotal = 0;
        var location = start;
        while (!endZs.contains(location)) {
          for (var codeUnit in directions.codeUnits) {
            stepTotal += 1;
            location = (codeUnit == codeUnitL) ? network[location]!.left : network[location]!.right;
            if (endZs.contains(location)) break;
          }
        }
        stepsRequired.add(stepTotal);
      }
      return _lcm(stepsRequired); // This works because the cycles as regular
    }

    var stepTotal = 0;
    var location = 'AAA';
    while (location != 'ZZZ') {
      for (var codeUnit in directions.codeUnits) {
        stepTotal += 1;
        location = (codeUnit == codeUnitL) ? network[location]!.left : network[location]!.right;
        if (location == 'ZZZ') break;
      }
    }

    return stepTotal;
  }

  int _lcm(List<int> numbers) {
    var lcm = numbers.first;
    for (var i = 1; i < numbers.length; i++) {
      lcm = (lcm ~/ (lcm.gcd(numbers[i])) * numbers[i]).abs();
    }
    return lcm;
  }
}
