using PotatoMilk;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace PotatoMilkDemo
{
    static class Storage
    {
        public static Texture texture;
        public static Texture texture2;
        public static Dictionary<string, ObjectRecipe> recipes;
        public static void Load()
        {
            texture = new Texture("textures.png");
            texture2 = new Texture("textures2.png");
            recipes = new();
            recipes.Add(
                "player_square",
                new ObjectRecipe()
                {
                    name = "player_square",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new()
                        {
                            ["position"] = new Vector2f(300f, 200f),
                        },
                        ["quad_renderer"] = new()
                        {
                            ["size"] = new Vector2f(16f, 16f),
                            ["texture"] = Storage.texture2,
                            ["texture_size"] = new Vector2f(32f, 32f),
                        },
                        ["convex_polygon_collider"] = new()
                        {
                            ["vertices"] = new List<Vector2f>() { new(), new(16f, 0f), new(16f, 16f), new(0f, 16f) },
                        },
                        ["player_square_beh"] = new()
                        {
                            ["test"] = "passed",
                        },
                    }
                }
            );
            recipes.Add(
                "player_triangle",
                new ObjectRecipe()
                {
                    name = "player_triangle",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new()
                        {
                            ["position"] = new Vector2f(10f, 0f),
                        },
                        ["quad_renderer"] = new()
                        {
                            ["texture_top_left"] = new Vector2f(0f, 32f),
                            ["size"] = new Vector2f(32f, 32f),
                            ["texture"] = Storage.texture,
                            ["texture_size"] = new Vector2f(32f, 32f),
                        },
                        ["convex_polygon_collider"] = new()
                        {
                            ["vertices"] = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 32f), new Vector2f(0f, 32f) },
                        },
                        ["player_triangle_beh"] = new(),
                    }
                }
            );
            recipes.Add(
                "polygon",
                new ObjectRecipe()
                {
                    name = "polygon",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new()
                        {
                            ["position"] = new Vector2f(200f, 100f),
                        },
                        ["polygon_renderer"] = new()
                        {
                            ["color"] = Color.Blue,
                            ["vertices"] = new List<Vector2f>() {
                                new (-19,-33),
                                new (56,-35),
                                new (93,12),
                                new (-11,59),
                                new (-98,37),
                                new (-80, -9),
                            },
                        },
                        ["convex_polygon_collider"] = new()
                        {
                            ["vertices"] = new List<Vector2f>() {
                                new (-19,-33),
                                new (56,-35),
                                new (93,12),
                                new (-11,59),
                                new (-98,37),
                                new (-80, -9),
                            },
                        },
                    }
                }
            );
            recipes.Add(
                "wall_circle",
                new ObjectRecipe()
                {
                    name = "wall_circle",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new()
                        {
                            ["position"] = new Vector2f(100f, 200f),
                        },
                        ["quad_renderer"] = new()
                        {
                            ["size"] = new Vector2f(32f, 32f),
                            ["texture"] = Storage.texture,
                            ["texture_size"] = new Vector2f(32f, 32f),
                            ["texture_top_left"] = new Vector2f(0f, 0f),
                        },
                        ["circle_collider"] = new()
                        {
                            ["radius"] = 16f,
                        },
                        ["wall_circle_beh"] = new(),
                    }
                }
            );
            recipes.Add(
                "wall_triangle",
                new ObjectRecipe()
                {
                    name = "wall_triangle",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new(),
                        ["quad_renderer"] = new()
                        {
                            ["size"] = new Vector2f(32f, 32f),
                            ["texture"] = Storage.texture,
                            ["texture_size"] = new Vector2f(32f, 32f),
                            ["texture_top_left"] = new Vector2f(32f, 32f),
                        },
                        ["convex_polygon_collider"] = new()
                        {
                            ["vertices"] = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 0f), new Vector2f(0f, 32f) },
                        },
                        ["wall_triangle_beh"] = new(),
                    }
                }
            );
            recipes.Add(
                "wall_square",
                new ObjectRecipe()
                {
                    name = "wall_square",
                    componentData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        ["transform"] = new(),
                        ["quad_renderer"] = new()
                        {
                            ["size"] = new Vector2f(32f, 32f),
                            ["texture"] = Storage.texture2,
                            ["texture_size"] = new Vector2f(32f, 32f),
                            ["texture_top_left"] = new Vector2f(32f, 0f),
                        },
                        ["convex_polygon_collider"] = new()
                        {
                            ["vertices"] = new List<Vector2f>() { new(0, 0), new(32, 0), new(32, 32), new(0, 32) },
                        },
                        ["wall_square_beh"] = new(),
                    }
                }
            );
        }
    }
}
