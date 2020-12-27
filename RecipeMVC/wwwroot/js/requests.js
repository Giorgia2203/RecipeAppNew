const ratingURI = "http://localhost:50541/api/Reviews"
const favouriteURI = "http://localhost:50541/api/FavouriteRecipes"

function addRating(event,userId,recipeId) {
    const star = event.target.id.split("-").pop()
    console.log(star)

    review = {
        appUserId: userId,
        recipeId: recipeId,
        rating: star
    };

    fetch(ratingURI, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(review)
    })
        .then(response => response.json())
        .then(() => {
            getAverageRatingForRecipe(recipeId)
            setRating(star)
        })
        .catch(error => console.error('Unable to add rating.', error));

}

function getRatingForUserAndRecipe(recipeId, userId) {
    fetch(ratingURI + "/" + recipeId + "/" + userId)
        .then(response => response.json())
        .then(data => setRating(data.rating))
        .catch(error => console.error("Error", error))
}

function setRating(rating) {
    starsReversed = document.getElementsByClassName("fa fa-star");
    length = starsReversed.length;
    stars = [];

    for (let index = 0; index < length; index++) {
        stars[index] = starsReversed[length - index - 1];
    }

    while (rating > 0) {
        stars[rating - 1].style.color = "#f3969a";
        rating--;
    }
}

function getAverageRatingForRecipe(recipeId) {
    fetch(ratingURI + "/" + recipeId)
        .then(response => response.json())
        .then(data => setAverage(data))
        .catch(error=> console.error("Error",error))
}

function setAverage(average) {
    averageElement = document.getElementById("average");

    averageElement.innerHTML = average + "/5";
}


function getIsFavourite(userId, recipeId) {
    fetch(favouriteURI + "/" + userId + "/" + recipeId)
        .then(response => response.json())
        .then(data => {
            if (data.status === 404) {
                setFavourite(false)
            } else {
                setFavourite(true)
            }
        })
        .catch(error => {
            setFavourite(false)
            console.error("Error", error)
        })
}

function setFavourite(isFavourite) {
    isFavouriteElement = document.getElementsByClassName("favourite")[0];

    if (isFavourite) {
        isFavouriteElement.style.color = "#e23232"
    }
}

function favouriteClicked(event,userId,recipeId) {
    heartColor = document.getElementsByClassName("favourite")[0].style.color
    console.log(heartColor)

    if (heartColor === "rgb(226, 50, 50)")
    {
        fetch(favouriteURI + "/" + userId + "/" + recipeId, {
            method: 'DELETE'
        })
            .then(() => setFavourite(false))
            .catch(error => console.error('Unable to delete favourite recipe.', error));
    }
    else
    {
        favouriteRecipe =
        {
            appUserId: userId,
            recipeId: recipeId
        };

        fetch(favouriteURI, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(favouriteRecipe)
        })
            .then(response => response.json())
            .then(() => setFavourite(true))
            .catch(error => console.error('Unable to add as favourite recipe.', error));
    }
}