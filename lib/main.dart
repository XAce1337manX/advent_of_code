import 'package:advent_of_code/2023/day_1/day_1.dart';
import 'package:advent_of_code/2023/day_2/day_2.dart';
import 'package:advent_of_code/2023/day_3/day_3.dart';
import 'package:advent_of_code/2023/day_4/day_4.dart';

Future<void> main() async {
  var day1 = Day4();
  print("Day 4");
  print(await day1.solve(isPart2: false));
  print(await day1.solve(isPart2: true));
  print("");
}
