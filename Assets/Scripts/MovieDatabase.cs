using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movie Database", menuName = "Movie Database")]
public class MovieDatabase : ScriptableObject
{
    public Movie[] Movies;
}
