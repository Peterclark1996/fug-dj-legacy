import { useMutation, useQueryClient } from "react-query"
import PlayerEnum from "../../Enums/PlayerEnum"
import { useApi } from "../../Hooks/ApiProvider"
import { useRoomHub } from "../../Hooks/RoomHubProvider"
import { BuildMediaResourceId } from "../../Logic"
import MediaData from "../../Types/MediaData"
import TagData from "../../Types/TagData"
import LinkButton from "./LinkButton"
import Loading from "../Loading"
import classes from "./Media.module.scss"
import StandardButton, { ButtonSize } from "../StandardButton"
import Tag from "../Tag/Tag"
import AddTag from "../Tag/AddTag"
import { useState } from "react"
import TagInput from "../Tag/TagInput"

type MediaProps = {
    media: MediaData,
    userTags: TagData[]
}

const GetUrlForMedia = (media: MediaData) => {
    switch (media.player) {
        case 0:
            return "https://www.youtube.com/watch?v=" + media.code
        default:
            throw new Error("Unknown player type")
    }
}

const Media = ({ media, userTags }: MediaProps) => {
    const queryClient = useQueryClient()
    const { apiPatch, apiDelete } = useApi()
    const { connection, connectedRoomId } = useRoomHub()

    const [isAddingTag, setIsAddingTag] = useState(false)
    const [newTagLabel, setNewTagLabel] = useState("")

    const onAddToQueueClick = () => {
        connection?.send('QueueMedia', connectedRoomId, media.player, media.code)
    }

    const deleteMediaMutation = useMutation(
        () => apiDelete(`user/deletemedia?media=${BuildMediaResourceId(media.player, media.code)}`),
        {
            onSuccess: () => {
                queryClient.invalidateQueries(["user"])
            }
        }
    )

    const deleteMediaTagMutation = useMutation(
        (tagId: number) => apiPatch(`user/updatemedia`, { ...media, tags: media.tags.filter(t => t !== tagId) }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries(["user"])
            }
        }
    )

    const onAddTagConfirmClick = () => {
        const tagToAdd = userTags.find(t => t.name === newTagLabel)
        if (tagToAdd === undefined) return

        addMediaTagMutation.mutate(tagToAdd.id)
        setIsAddingTag(false)
    }

    const onAddConfirmClick = () => {
        const tagToAdd = userTags.find(t => t.name.toLowerCase() === newTagLabel.toLowerCase())
        if (tagToAdd === undefined) return

        addMediaTagMutation.mutate(tagToAdd.id)
        setIsAddingTag(false)
    }

    const onAddCancelClick = () => {
        setIsAddingTag(false)
    }

    const addMediaTagMutation = useMutation(
        (tagId: number) => apiPatch(`user/updatemedia`, { ...media, tags: [...media.tags, tagId] }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries(["user"])
            }
        }
    )

    const onAddTagClick = () => {
        setNewTagLabel("")
        setIsAddingTag(true)
    }

    return (
        <div className={`d-flex flex-column m-1 p-1 user-select-none rounded ${classes.shadow}`}>
            <Loading isLoading={deleteMediaMutation.isLoading || deleteMediaTagMutation.isLoading}>
                <div className="d-flex">
                    <span className={classes.largeFont}>{media.name}</span>
                    <StandardButton className="ms-auto p-2" iconClasses="fa-solid fa-trash-can" size={ButtonSize.SMALL} onClick={deleteMediaMutation.mutate} />
                </div>
                <div className="d-flex my-1">
                    {
                        media.tags
                            .map(tag => userTags.find(userTag => userTag.id === tag))
                            .map((tagData, index) => {
                                return (
                                    tagData && <Tag key={index} tag={tagData} onClick={() => deleteMediaTagMutation.mutate(tagData.id)} />
                                )
                            })
                    }
                    {
                        isAddingTag ?
                            <TagInput
                                label={newTagLabel}
                                onLabelChange={setNewTagLabel}
                                availableTags={userTags}
                                colourHex="d9d2e9"
                                onAddConfirmClick={onAddConfirmClick}
                                onAddCancelClick={onAddCancelClick}
                            /> :
                            <AddTag
                                onClick={onAddTagClick}
                            />
                    }

                </div>
                <div className="d-flex justify-content-between mt-auto">
                    <StandardButton className="py-0 px-1 text-white" text="Add to Queue" onClick={onAddToQueueClick} />
                    <LinkButton linkUrl={GetUrlForMedia(media)} text={PlayerEnum[media.player]} />
                </div>
            </Loading>
        </div>
    )
}

export default Media