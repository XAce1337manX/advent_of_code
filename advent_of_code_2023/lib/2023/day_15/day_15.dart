import 'dart:io';

import 'package:collection/collection.dart';

class Day15 {
  final dashCodeUnit = '-'.codeUnitAt(0);
  final equalsCodeUnit = '='.codeUnitAt(0);
  final operationExp = RegExp(r'[=-]');

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_15/day_15_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var line = lines[0];

    var sequence = line.split(',');
    var valueTotal = 0;

    if (!isPart2) {
      var valueTotal = sequence.fold(0, (previousValue, element) => previousValue + _hash(element));
      return valueTotal;
    }

    var boxes = <int, List<({String label, int focalLength})>>{};
    for (var step in sequence) {
      var split = step.split(operationExp);

      var label = split[0];
      var labelHash = _hash(label);
      var focalLength = int.tryParse(split[1]);

      if (focalLength != null) {
        var lens = (label: label, focalLength: focalLength);
        if (boxes.containsKey(labelHash)) {
          var box = boxes[labelHash]!;
          var existingLensIndex = box.indexWhere((lens) => lens.label == label);
          if (existingLensIndex != -1) {
            box[existingLensIndex] = lens;
          } else {
            boxes[labelHash]!.add(lens);
          }
        } else {
          boxes[labelHash] = [lens];
        }
      } else if (boxes.containsKey(labelHash)) {
        boxes[labelHash]!.removeWhere((lens) => lens.label == label);
      }
    }

    for (var entry in boxes.entries) {
      valueTotal += entry.value.foldIndexed(
        0,
        (i, previous, lens) => previous + (entry.key + 1) * (i + 1) * lens.focalLength,
      );
    }

    return valueTotal;
  }

  int _hash(String string) {
    var value = 0;
    for (var char in string.codeUnits) {
      value += char;
      value *= 17;
      value %= 256;
    }
    return value;
  }
}
