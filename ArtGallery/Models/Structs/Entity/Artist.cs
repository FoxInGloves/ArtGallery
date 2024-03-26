﻿#nullable disable
namespace ArtGallery.Models.Structs.Entity;

public class Artist
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Country { get; set; }
    
    public string IconPath { get; set; }
}