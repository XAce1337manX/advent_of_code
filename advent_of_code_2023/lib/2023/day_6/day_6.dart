import 'dart:io';

class Day6 {
  final numberExp = RegExp(r'\d+');

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_6/day_6_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var fileTimes = isPart2 ? lines[0].replaceAll(RegExp(r' '), '') : lines[0];
    var fileDistances = isPart2 ? lines[1].replaceAll(RegExp(r' '), '') : lines[0];

    var times = numberExp.allMatches(fileTimes).map((match) => int.parse(match.group(0)!)).toList();
    var distances =
        numberExp.allMatches(fileDistances).map((match) => int.parse(match.group(0)!)).toList();

    var permutations = 1;
    for (var i = 0; i < times.length; i++) {
      var waysToBeatRecord = 0;
      for (var j = 0; j <= times[i]; j++) {
        var score = (times[i] - j) * j;
        if (score > distances[i]) {
          waysToBeatRecord++;
        }
      }

      permutations *= waysToBeatRecord;
    }

    return permutations;
  }
}
