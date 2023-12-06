import 'dart:async';
import 'dart:io';

class Day1 {
  Future<int> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_1/day_1_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var calibrationTotal = 0;

    for (var line in lines) {
      if (isPart2) {
        // We surround number with first and last character because oneight means 18.
        line = line.replaceAll(RegExp(r'one'), 'o1e');
        line = line.replaceAll(RegExp(r'two'), 't2o');
        line = line.replaceAll(RegExp(r'three'), 't3e');
        line = line.replaceAll(RegExp(r'four'), 'f4r');
        line = line.replaceAll(RegExp(r'five'), 'f5e');
        line = line.replaceAll(RegExp(r'six'), 's6x');
        line = line.replaceAll(RegExp(r'seven'), 's7n');
        line = line.replaceAll(RegExp(r'eight'), 'e8t');
        line = line.replaceAll(RegExp(r'nine'), 'n9e');
      }
      var numericLine = line.replaceAll(RegExp(r'\D'), '');
      var calibrationValue = int.parse(numericLine[0] + numericLine[numericLine.length - 1]);
      calibrationTotal += calibrationValue;
    }

    return calibrationTotal;
  }
}
