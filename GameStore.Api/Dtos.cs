using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record GameDto(
				int Id,
				string Name,
				string Genre,
				decimal Price,
				DateTime ReleaseDate,
				string ImageUrl
				);

public record CreateGameDto(
				[Required] string Name,
				[Required] string Genre,
				[Range(1, 100)] decimal Price,
				DateTime ReleaseDate,
				[Url][StringLength(100)] string ImageUrl
				);

public record UpdateGameDto(
				[Required] string Name,
				[Required] string Genre,
				[Range(1, 100)] decimal Price,
				DateTime ReleaseDate,
				[Url][StringLength(100)] string ImageUrl
				);
