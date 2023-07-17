CREATE OR ALTER FUNCTION dbo.GetUnmatchedUsers(@Count INT, @UserID INT)
RETURNS TABLE
AS
RETURN (
	SELECT TOP(@Count) *
	FROM dbo.[User] u1
	WHERE u1.ID != @UserID
		AND (
			u1.ID NOT IN (
				SELECT UserID2
				FROM dbo.UserLikes
				WHERE UserID1 = @UserID
			) OR EXISTS (
				SELECT *
				FROM dbo.UserLikes ul1
				JOIN dbo.UserLikes ul2 ON ul1.UserID1 = ul2.UserID2
					AND ul1.UserID2 = ul2.UserID1
					AND ul1.Smash = 0
					AND ul2.Smash = 0
				WHERE ul1.UserID1 = u1.ID
					AND ul1.UserID2 = @UserID
			)
	)
)
GO

SELECT *
FROM dbo.GetUnmatchedUsers(1024, 2);

UPDATE dbo.UserLikes
SET Smash = 1
WHERE UserID1 = 2
	AND UserID2 = 5;

SELECT *
FROM dbo.UserLikes;

DELETE FROM dbo.userlikes;