using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DotParser {
  public class Graph {
    public List<State> States { get; set; } = new List<State>();
    public List<Transition> Transitions { get; set; } = new List<Transition>();
  }

  public class State {
    public string Name { get; set; }
    public Dictionary<string, string> Properties { get; set; }
  }

  public class Transition {
    public string Source { get; set; }
    public string Destination { get; set; }
    public Dictionary<string, string> Properties { get; set; }
  }

  public Graph Parse(string filePath) {
    string content = File.ReadAllText(filePath);

    //strip comments
    content = Regex.Replace(content, @"//.*", "");

    Graph graph = new Graph();

    ParseStates(content, graph);
    ParseTransitions(content, graph);

    return graph;
  }

  private void ParseStates(string content, Graph graph) {
    // Regular expression to extract node definitions
    MatchCollection matches = Regex.Matches(content, @"(?:\n *)(\w+)\s*(?:\[(.*?)\])?;");

    foreach (Match match in matches) {
      State state = new() {
        Name = match.Groups[1].Value,
        Properties = (match.Groups[2] is not null) ? ParseProperties(match.Groups[2].Value) : new Dictionary<string, string>()
      };

      graph.States.Add(state);
    }

    CleanUp(graph);
  }

  private void ParseTransitions(string content, Graph graph) {
    // Regular expression to extract edge definitions
    MatchCollection matches = Regex.Matches(content, @"(\w+)\s*->\s*(\w+)\s*(?:\[(.*?)\])?;");

    foreach (Match match in matches) {
      Transition transition = new() {
        Source = match.Groups[1].Value,
        Destination = match.Groups[2].Value,
        Properties = (match.Groups[3] is not null) ? ParseProperties(match.Groups[3].Value) : new Dictionary<string, string>()
      };

      if(!graph.States.Exists(state => state.Name == transition.Source)) {
        graph.States.Add(new State() {
          Name = transition.Source,
          Properties = new Dictionary<string, string>()
        });
      }

      if(!graph.States.Exists(state => state.Name == transition.Destination)) {
        graph.States.Add(new State() {
          Name = transition.Destination,
          Properties = new Dictionary<string, string>()
        });
      }

      graph.Transitions.Add(transition);
    }
  }

  private Dictionary<string, string> ParseProperties(string value) {
    Dictionary<string, string> properties = new Dictionary<string, string>();

    MatchCollection matches = Regex.Matches(value, @"(\w+)\s*=\s*(.*?);");

    foreach (Match match in matches) {
      properties.Add(match.Groups[1].Value, match.Groups[2].Value);
    }

    return properties;
  }

  private void CleanUp(Graph graph) {
    graph.States.RemoveAll(state => state.Name == "node" || state.Name == "edge" || state.Name == "graph");
  }
}