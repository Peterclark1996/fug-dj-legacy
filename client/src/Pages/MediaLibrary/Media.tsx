import { useMutation, useQueryClient } from "react-query"
import PlayerEnum from "../../Enums/PlayerEnum"
import { useApi } from "../../Hooks/ApiProvider"
import MediaData from "../../Types/MediaData.d"
import TagData from "../../Types/TagData"
import LinkButton from "./LinkButton"
import Loading from "../../Components/Loading"
import classes from "./Media.module.scss"
import StandardButton, { ButtonSize } from "../../Components/StandardButton"
import Tag from "./Tag/Tag"
import AddTag from "./Tag/AddTag"
import { useState } from "react"
import TagInput from "./Tag/TagInput"
import { useMediaQueue } from "../../Hooks/MediaQueueProvider"
import { Endpoint, getMediaUrl, getYoutubeUrl, Resource } from "../../Constants"

type MediaProps = {
    media: MediaData,
    userTags: TagData[]
}

const GetUrlForMedia = (media: MediaData) => {
    switch (media.player) {
        case 0:
            return getYoutubeUrl(media.code)
        default:
            throw new Error("Unknown player type")
    }
}

const Media = ({ media, userTags }: MediaProps) => {
    const queryClient = useQueryClient()
    const { apiPost, apiPatch, apiDelete } = useApi()
    const { addToQueue } = useMediaQueue()

    const [isAddingTag, setIsAddingTag] = useState(false)

    const deleteMediaMutation = useMutation(
        () => apiDelete(getMediaUrl(media.player, media.code)),
        {
            onSuccess: () => {
                queryClient.invalidateQueries([Resource.USER])
            }
        }
    )

    const deleteMediaTagMutation = useMutation(
        (tagId: number) => apiPatch(getMediaUrl(media.player, media.code), { name: media.name, tags: media.tags.filter(t => t !== tagId) }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries([Resource.USER])
            }
        }
    )

    const onAddConfirmClick = (tagName: string) => {
        const tagToAdd = userTags.find(t => t.name.toLowerCase() === tagName.toLowerCase())

        if (tagToAdd === undefined) {
            createTagMutation.mutate(tagName)
        } else {
            addMediaTagMutation.mutate(tagToAdd.id)
        }

        setIsAddingTag(false)
    }

    const onAddCancelClick = () => {
        setIsAddingTag(false)
    }

    const addMediaTagMutation = useMutation(
        (tagId: number) => apiPatch(getMediaUrl(media.player, media.code), { name: media.name, tags: [...media.tags, tagId] }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries([Resource.USER])
            }
        }
    )

    const createTagMutation = useMutation(
        (tagName: string) => apiPost(Endpoint.POST_CREATE_MEDIA_TAG, { mediaToAddTagTo: { player: media.player, code: media.code }, tagName }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries([Resource.USER])
            }
        }
    )

    const onAddTagClick = () => {
        setIsAddingTag(true)
    }

    return (
        <div className={`d-flex flex-column m-1 p-2 user-select-none rounded ${classes.shadow}`}>
            <Loading isLoading={deleteMediaMutation.isLoading || deleteMediaTagMutation.isLoading}>
                <div className="d-flex">
                    <span className={classes.largeFont}>{media.name}</span>
                    <StandardButton
                        className="ms-auto p-2"
                        iconClasses="fa-solid fa-trash-can"
                        toolTipText="Delete"
                        size={ButtonSize.SMALL}
                        onClick={deleteMediaMutation.mutate}
                    />
                </div>
                <div className="d-flex flex-wrap my-1">
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
                                availableTags={userTags.filter(t => !media.tags.includes(t.id))}
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
                    <StandardButton className="py-0 px-1 text-white" text="Add to Queue" onClick={() => addToQueue(media)} />
                    <LinkButton linkUrl={GetUrlForMedia(media)} text={PlayerEnum[media.player]} />
                </div>
            </Loading>
        </div>
    )
}

export default Media