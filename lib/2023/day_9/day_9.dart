import 'dart:io';

class Day9 {
  final numberExp = RegExp(r'\d+');

  Future<num> solve({bool isPart2 = false}) async {
    var path = "lib/2023/day_9/day_9_input.txt";
    var file = File(path);
    var lines = await file.readAsLines();

    var total = 0;

    for (var line in lines) {
      var numbers = line.split(' ').map((e) => int.parse(e)).toList();
      var triangle = _extrapolate(numbers);

      var triangleTotal = 0;
      for (var element in triangle) {
        if (isPart2) {
          triangleTotal = element.first - triangleTotal;
        } else {
          triangleTotal = element.last + triangleTotal;
        }
      }
      total += triangleTotal;
    }

    return total;
  }

  List<List<int>> _extrapolate(List<int> numbers) {
    var triangle = <List<int>>[];
    triangle.add(numbers);

    for (var i = 0; i < numbers.length - 1; i++) {
      var row = triangle[i];
      var nextRow = <int>[];

      for (var j = 0; j < row.length - 1; j++) {
        nextRow.add(row[j + 1] - row[j]);
      }

      triangle.add(nextRow);
    }

    return triangle;
  }
}
