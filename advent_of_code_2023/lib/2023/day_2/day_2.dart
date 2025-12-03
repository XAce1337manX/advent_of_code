import 'dart:io';
import 'dart:math';

class Day2 {
  final redMax = 12;
  final greenMax = 13;
  final blueMax = 14;

  final redKey = "red";
  final blueKey = "blue";
  final greenKey = "green";

  final numberExp = RegExp(r'\d+');

  Future<int> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_2/day_2_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var validGameIdTotal = 0;
    var powerSetTotal = 0;

    for (var line in lines) {
      var gameId = int.parse(numberExp.firstMatch(line)![0]!);
      var gameMinimuns = {
        blueKey: 0,
        redKey: 0,
        greenKey: 0,
      };

      var colonLocation = line.indexOf(':');
      var sets = line.substring(colonLocation + 1).split(';');

      var gameIsValid = true;

      for (var set in sets) {
        var setTotals = {
          blueKey: 0,
          redKey: 0,
          greenKey: 0,
        };

        var reveals = set.trim().split(',');
        for (var reveal in reveals) {
          var splitReveal = reveal.trim().split(' ');
          var number = int.parse(splitReveal[0]);
          var colour = splitReveal[1];

          gameMinimuns[colour] = max(gameMinimuns[colour]!, number);
          setTotals[colour] = setTotals[colour]! + number;
        }

        gameIsValid = gameIsValid &&
            setTotals[blueKey]! <= blueMax &&
            setTotals[redKey]! <= redMax &&
            setTotals[greenKey]! <= greenMax;
      }

      powerSetTotal += _productOfValues(gameMinimuns);

      if (gameIsValid) {
        validGameIdTotal += gameId;
      }
    }

    return isPart2 ? powerSetTotal : validGameIdTotal;
  }

  int _productOfValues(Map<Object, int> numbers) => numbers.values.reduce((a, b) => a * b);
}
