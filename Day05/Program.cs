using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 5: If You Give A Seed A Fertilizer"));
            Console.WriteLine("Almanac: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            extractMappings(puzzleInput.Lines, out List<Range> seeds, out List<Mapping> seedToSoil, out List<Mapping> soilToFertilizer, out List<Mapping> fertilizerToWater, out List<Mapping> waterToLight,
                                out List<Mapping> lightToTemperature, out List<Mapping> temperatureToHumidity, out List<Mapping> humidityToLocation);

            Int64 minLocation = determineLocationNumbers(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
            Console.WriteLine("Smallest location number: {0}", minLocation);

            minLocation = determineLocationNumberSeedRange(seeds, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);
            Console.WriteLine("Smallest location number: {0}", minLocation);
        }

        private static Int64 determineLocationNumberSeedRange(List<Range> seeds, List<Mapping> seedToSoil, List<Mapping> soilToFertilizer, List<Mapping> fertilizerToWater, List<Mapping> waterToLight, List<Mapping> lightToTemperature, List<Mapping> temperatureToHumidity, List<Mapping> humidityToLocation)
        {
            List<Int64> locationNumbers = new List<Int64>();
            List<Range> seedRange = new List<Range>();

            for (int i = 0; i < seeds.Count; i=i+2)
            {
                Range seed = new Range(seeds[i].Start, seeds[i+1].Start);
                seedRange.Add(seed);
            }

            locationNumbers.Add(determineLocationNumbers(seedRange, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation));

            return locationNumbers.Min();
        }

        private static Int64 determineLocationNumbers(List<Range> seeds, List<Mapping> seedToSoil, List<Mapping> soilToFertilizer, List<Mapping> fertilizerToWater, List<Mapping> waterToLight,
                                            List<Mapping> lightToTemperature, List<Mapping> temperatureToHumidity, List<Mapping> humidityToLocation)
        {
            List<Range> soils = new List<Range>();
            List<Range> fertilizers = new List<Range>();
            List<Range> waters = new List<Range >();
            List<Range> lights = new List<Range>();
            List<Range> temperatures = new List<Range>();
            List<Range> humidities = new List<Range>();
            List<Range> locations = new List<Range>();

            foreach(Range seed in seeds)
            {
                 soils.AddRange(getMappingValue(seed, seedToSoil));
            }
            
            foreach (Range soil in soils)
            {
                fertilizers.AddRange(getMappingValue(soil, soilToFertilizer));
            }
            foreach (Range fertilizer in fertilizers)
            {
                waters.AddRange(getMappingValue(fertilizer, fertilizerToWater));
            }
            foreach (Range water in waters)
            {
                lights.AddRange(getMappingValue(water, waterToLight));
            }
            foreach (Range light in lights)
            {
                temperatures.AddRange(getMappingValue(light, lightToTemperature));
            }
            foreach (Range temperature in temperatures)
            {
                humidities.AddRange(getMappingValue(temperature, temperatureToHumidity));
            }
            foreach (Range humidity in humidities)
            {
                locations.AddRange(getMappingValue(humidity, humidityToLocation));
            }

            return locations.Min( l=> l.Start);
        }

        private static List<Range> getMappingValue(Range sourceRange, List<Mapping> mapping)
        {
            List<Range> result = new List<Range>();

            // range is fully included in one map
            Mapping? map = mapping.Where(m => m.Source.Start <= sourceRange.Start && m.Source.End >= sourceRange.End).FirstOrDefault();
            if (map is not null)
            {
                result.Add(new Range(sourceRange.Start, map, sourceRange.Length));
            }
            else
            {
                // range does not overlap with any map
                map = mapping.Where(m => sourceRange.Start <= m.Source.End && sourceRange.End >= m.Source.Start).FirstOrDefault();
                if (map is null)
                {
                    result.Add(new Range(sourceRange.Start, sourceRange.Length));
                }
                else
                {
                    // range is overlapping, start of range is smaller than map
                    map = mapping.Where(m => sourceRange.Start < m.Source.Start && sourceRange.End >= m.Source.Start && sourceRange.End <= m.Source.End).OrderBy(m => m.DestinationStart).FirstOrDefault();
                    if (map is not null)
                    {
                        result.AddRange(getMappingValue(new Range(sourceRange.Start, map.Source.Start - sourceRange.Start), mapping));
                        result.AddRange(getMappingValue(new Range(map.Source.Start, sourceRange.End - map.Source.Start + 1), mapping));
                    }
                    else 
                    {
                        // range is overlapping, start of range is greater than map
                        map = mapping.Where(m => sourceRange.Start >= m.Source.Start && sourceRange.End >= m.Source.Start && sourceRange.Start <= m.Source.End).OrderBy(m => m.DestinationStart).FirstOrDefault();
                        if (map is not null)
                        {
                            result.AddRange(getMappingValue(new Range(sourceRange.Start, map.Source.End - sourceRange.Start + 1), mapping));
                            result.AddRange(getMappingValue(new Range(map.Source.End + 1, sourceRange.End - map.Source.End), mapping));
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }

            
            

            return result;
        }

        private static void extractMappings(List<string> lines, out List<Range> seeds, out List<Mapping> seedToSoil, out List<Mapping> soilToFertilizer, out List<Mapping> fertilizerToWater, out List<Mapping> waterToLight,
                                            out List<Mapping> lightToTemperature, out List<Mapping> temperatureToHumidity, out List<Mapping> humidityToLocation)
        {
            seeds = extractSeeds(lines[0]);
            seedToSoil = extractMappings(lines, "seed-to-soil map");
            soilToFertilizer = extractMappings(lines, "soil-to-fertilizer map");
            fertilizerToWater = extractMappings(lines, "fertilizer-to-water map");
            waterToLight = extractMappings(lines, "water-to-light map");
            lightToTemperature = extractMappings(lines, "light-to-temperature map");
            temperatureToHumidity = extractMappings(lines, "temperature-to-humidity map");
            humidityToLocation = extractMappings(lines, "humidity-to-location map");                      
        }

        private static List<Mapping> extractMappings(List<string> lines, string map)
        {
            List<Mapping> mappings = new List<Mapping>();
            int lineIndex = lines.FindIndex(line => line.StartsWith(map));
            if (lineIndex == -1) { throw new ArgumentException(); }

            lineIndex++;
            while (lineIndex < lines.Count && lines[lineIndex].Trim() != "")
            {
                string[] items = lines[lineIndex].Split(' ');

                mappings.Add(new Mapping(Int64.Parse(items[0]), Int64.Parse(items[1]), Int64.Parse(items[2])));

                lineIndex++;
            }

            return mappings;
        }

        private static List<Range> extractSeeds(string line)
        {
            List<Range> seeds = new List<Range>();
            line = line.Replace("seeds: ", "");
            line = line.Trim();
            List<Int64> values = line.Split(' ').Select(n => Int64.Parse(n)).ToList();
            foreach (Int64 value in values)
            {
                Range seed = new Range(value, 1);
                seeds.Add(seed);
            }

            return seeds;
        }

        
    }
}