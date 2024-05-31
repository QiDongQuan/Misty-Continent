using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    public LinkedList<BuffInfo> buffList = new LinkedList<BuffInfo>();

    private void Update()
    {
        BuffTickAndRemove();
    }

    private void BuffTickAndRemove()
    {
        List<BuffInfo> deleteBuffList = new List<BuffInfo>();
        foreach (var buffInfo in buffList)
        {
            if (buffInfo.buffData.OnTick != null)
            {
                if (buffInfo.buffData.isSkill && buffInfo.target.CompareTag("Player"))
                {
                    if (buffInfo.tickTimer < 0)
                    {
                        if (buffInfo.target.GetComponent<PlayerCharacter>().target)
                        {
                            buffInfo.buffData.OnTick?.Applay(buffInfo);
                            buffInfo.tickTimer = buffInfo.buffData.tickTime;
                        }
                    }
                    else
                    {
                        buffInfo.tickTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    if (buffInfo.tickTimer < 0)
                    {
                        buffInfo.buffData.OnTick?.Applay(buffInfo);
                        buffInfo.tickTimer = buffInfo.buffData.tickTime;
                    }
                    else
                    {
                        buffInfo.tickTimer -= Time.deltaTime;
                    }
                }
            }

            if (buffInfo.buffData.isForever)
            {
                continue;
            }
            if (buffInfo.durationTimer < 0)
            {
                deleteBuffList.Add(buffInfo);
            }
            else
            {
                buffInfo.durationTimer -= Time.deltaTime;
            }
        }

        foreach (var buffInfo in deleteBuffList)
        {
            RemoveBuff(buffInfo);
        }
    }

    public void AddBuff(BuffInfo buffInfo)
    {
        BuffInfo findBuffInfo = FindBuff(buffInfo.buffData.id);
        if (findBuffInfo != null)
        {
            if (findBuffInfo.curStack < findBuffInfo.buffData.maxStack)
            {
                findBuffInfo.curStack++;
                switch (findBuffInfo.buffData.buffUpdateTime)
                {
                    case BuffUpdateTimeEnum.Add:
                        findBuffInfo.durationTimer += findBuffInfo.buffData.duration;
                        break;
                    case BuffUpdateTimeEnum.Replace:
                        findBuffInfo.durationTimer = findBuffInfo.buffData.duration;
                        break;
                }
                findBuffInfo.buffData.OnCreate?.Applay(findBuffInfo);
            }
            else
            {
                findBuffInfo.durationTimer = findBuffInfo.buffData.duration * findBuffInfo.curStack;
                findBuffInfo.buffData.OnCreate?.Applay(findBuffInfo);
            }
        }
        else
        {
            buffInfo.durationTimer = buffInfo.buffData.duration;
            //buffInfo.tickTimer = buffInfo.buffData.tickTime;
            buffInfo.buffData.OnCreate?.Applay(buffInfo);
            buffList.AddLast(buffInfo);
            InsertionSortLinkedList(buffList);
        }
    }

    public void RemoveBuff(BuffInfo buffInfo)
    {
        switch (buffInfo.buffData.buffRemoveStackUpdata)
        {
            case BuffRemoveStackUpdataEnum.Clear:
                buffInfo.buffData.OnRemove?.Applay(buffInfo);
                buffList.Remove(buffInfo);
                break;
            case BuffRemoveStackUpdataEnum.Reduce:
                buffInfo.curStack--;
                buffInfo.buffData.OnRemove?.Applay(buffInfo);
                if (buffInfo.curStack == 0)
                {
                    buffList.Remove(buffInfo);
                }
                else
                {
                    buffInfo.durationTimer = buffInfo.buffData.duration;
                }
                break;
        }
    }

    private BuffInfo FindBuff(int buffDataID)
    {
        foreach (BuffInfo buffInfo in buffList)
        {
            if (buffInfo.buffData.id == buffDataID)
            {
                return buffInfo;
            }
        }

        return default;
    }

    //²åÈëÅÅÐò
    void InsertionSortLinkedList(LinkedList<BuffInfo> list)
    {
        if (list == null || list.First == null)
        {
            return;
        }

        LinkedListNode<BuffInfo> current = list.First.Next;

        while (current != null)
        {
            LinkedListNode<BuffInfo> next = current.Next;
            LinkedListNode<BuffInfo> prev = current.Previous;

            while (prev != null && prev.Value.buffData.priority > current.Value.buffData.priority)
            {
                prev = prev.Previous;
            }

            if (prev == null)
            {
                list.Remove(current);
                list.AddFirst(current);
            }
            else
            {
                list.Remove(current);
                list.AddAfter(prev, current);
            }

            current = next;
        }
    }
}
