import 'package:advent_of_code/2023/day_1/day_1.dart';

Future<void> main() async {
  var day1 = Day1();
  print("Day 1");
  print(await day1.solve(isPart2: false));
  print(await day1.solve(isPart2: true));
  print("");
}
